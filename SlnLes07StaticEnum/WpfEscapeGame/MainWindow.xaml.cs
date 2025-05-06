using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEscapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Room currentRoom; // will become useful in later versions
        private List<Item> inventory;
        List<Item> myItems = new List<Item>(); // Player's inventory
        Item selectedRoomItem;
        Item selectedInventoryItem;

        public MainWindow()
        {
            InitializeComponent();


            // define room
            Room room1 = new Room()
            {
                Name = "bedroom",
                Description = "I seem to be in a medium sized bedroom. There is a locker to the left, a nice rug on the floor, a bed to the right, a chair in the corner, and a poster on the wall."
            };

            // define items
            Item key1 = new Item()
            {
                Name = "small silver key",
                Description = "A small silver key, makes me think of one I had at highschool."
            };

            Item key2 = new Item()
            {
                Name = "large key",
                Description = "A large key. Could this be my way out?"
            };

            Item locker = new Item()
            {
                Name = "locker",
                Description = "A locker. I wonder what's inside.",
                IsPortable = false
            };
            locker.HiddenItem = key2;
            locker.IsLocked = true;
            locker.Key = key1;

            Item bed = new Item()
            {
                Name = "bed",
                Description = "Just a bed. I am not tired now.",
                IsPortable = false
            };
            bed.HiddenItem = key1;

            // Create chair item
            Item chair = new Item()
            {
                Name = "chair",
                Description = "A wooden chair. Looks sturdy but nothing special about it.",
                IsPortable = false
            };

            // Create poster item
            Item poster = new Item()
            {
                Name = "poster",
                Description = "A movie poster for 'The Great Escape'. Somehow feels appropriate for my current situation."
            };

            // setup bedroom
            room1.Items.Add(new Item()
            {
                Name = "floor mat",
                Description = "A bit ragged floor mat, but still one of the most popular designs.",
                IsPortable = false
            });
            room1.Items.Add(bed);
            room1.Items.Add(locker);
            room1.Items.Add(chair);
            room1.Items.Add(poster);
            // Define other rooms
Room livingRoom = new Room()
{
    Name = "living room",
    Description = "A cozy living room with a couch, a coffee table, and a door to the left."
};

Room computerRoom = new Room()
{
    Name = "computer room",
    Description = "A small room filled with tech gear and humming servers."
};

// Add some items to living room
livingRoom.Items.Add(new Item()
{
    Name = "remote control",
    Description = "A remote control for something. Maybe the TV?"
});
livingRoom.Items.Add(new Item()
{
    Name = "sofa",
    Description = "A comfy sofa. No hidden coins here, sadly.",
    IsPortable = false
});

// Add some items to computer room
computerRoom.Items.Add(new Item()
{
    Name = "usb stick",
    Description = "A USB stick labeled 'Secret Data'. Might be useful later."
});
computerRoom.Items.Add(new Item()
{
    Name = "monitor",
    Description = "A black screen. It doesn't seem to be on.",
    IsPortable = false
});

// Define doors
Door doorToLiving = new Door()
{
    Name = "door to living room",
    Description = "A heavy door. Feels locked.",
    IsLocked = true,
    Key = key2,
    ToRoom = livingRoom
};

Door doorToComputer = new Door()
{
    Name = "door to computer room",
    Description = "A wooden door. It's open.",
    IsLocked = false,
    ToRoom = computerRoom
};

Door doorBackToLiving = new Door()
{
    Name = "door to living room",
    Description = "Back to the living room.",
    IsLocked = false,
    ToRoom = livingRoom
};

Door exitDoor = new Door()
{
    Name = "exit door",
    Description = "This must be the way out... but it's locked tight.",
    IsLocked = true,
    ToRoom = null
};

// Add doors to rooms
room1.Doors.Add(doorToLiving);
livingRoom.Doors.Add(doorToComputer);
computerRoom.Doors.Add(doorBackToLiving);
livingRoom.Doors.Add(exitDoor);

            room1.ImagePath = "Images/bedroom.png";
            livingRoom.ImagePath = "Images/livingroom.png";
            computerRoom.ImagePath = "Images/computerroom.png";


            // start game
            currentRoom = room1;
            txtMessage.Text = "I am awake, but cannot remember who I am!? Must have been a hell of a party last night...";
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();
        }

        /// <summary>
        /// Update the items in the ListBoxes
        /// </summary>
        private void UpdateUI()
        {
            lstRoomItems.Items.Clear();
            foreach (Item itm in currentRoom.Items)
            {
                lstRoomItems.Items.Add(itm);
            }

            lstMyItems.Items.Clear();
            foreach (Item itm in myItems)
            {
                lstMyItems.Items.Add(itm);
            }

            DoorListBox.Items.Clear();
            foreach (Door door in currentRoom.Doors)
            {
                DoorListBox.Items.Add(door);
            }

            // Reset selection and disable buttons
            btnCheck.IsEnabled = false;
            btnPickUp.IsEnabled = false;
            btnUseOn.IsEnabled = false;
            btnDrop.IsEnabled = false;

            selectedRoomItem = null;
            selectedInventoryItem = null;

            // Disable door buttons initially
            OpenDoorButton.IsEnabled = false;
            EnterRoomButton.IsEnabled = false;

            DoorListBox.ItemsSource = null;
            DoorListBox.ItemsSource = currentRoom.Doors;

            if (!string.IsNullOrEmpty(currentRoom.ImagePath))
            {
                RoomImage.Source = new BitmapImage(new Uri(currentRoom.ImagePath, UriKind.Relative));
            }
            else
            {
                RoomImage.Source = null;
            }

        }


        /// <summary>
        /// Handle selection changes in both ListBoxes
        /// </summary>
        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update selected items and button states
            ListBox listBox = sender as ListBox;

            if (listBox == lstRoomItems)
            {
                selectedRoomItem = lstRoomItems.SelectedItem as Item;
                btnCheck.IsEnabled = selectedRoomItem != null;
                btnPickUp.IsEnabled = selectedRoomItem != null;
            }
            else if (listBox == lstMyItems)
            {
                selectedInventoryItem = lstMyItems.SelectedItem as Item;
                btnDrop.IsEnabled = selectedInventoryItem != null;
            }

            // The Use On button is enabled only when both an inventory item and room item are selected
            btnUseOn.IsEnabled = selectedRoomItem != null && selectedInventoryItem != null;
        }

        /// <summary>
        /// Check/examine an item in the room
        /// </summary>
        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoomItem != null)
            {
                txtMessage.Text = selectedRoomItem.Description;

                // Check if there's a hidden item that's not locked
                if (selectedRoomItem.HiddenItem != null && !selectedRoomItem.IsLocked)
                {
                    txtMessage.Text += $"\nI found a {selectedRoomItem.HiddenItem.Name}!";
                    currentRoom.Items.Add(selectedRoomItem.HiddenItem);
                    selectedRoomItem.HiddenItem = null;
                    UpdateUI();
                }
            }
        }

        /// <summary>
        /// Pick up an item from the room
        /// </summary>
        private void BtnPickUp_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRoomItem != null)
            {
                // Check if the item can be picked up
                if (!selectedRoomItem.IsPortable)
                {
                    txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.ItemNotPortable);
                }
                else
                {
                    txtMessage.Text = $"I picked up the {selectedRoomItem.Name}.";
                    myItems.Add(selectedRoomItem);
                    currentRoom.Items.Remove(selectedRoomItem);
                    UpdateUI();
                }
            }
        }

        /// <summary>
        /// Use an inventory item on a room item
        /// </summary>
        private void BtnUseOn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInventoryItem != null && selectedRoomItem != null)
            {
                // Check if the inventory item is a key for the room item
                if (selectedRoomItem.IsLocked && selectedRoomItem.Key == selectedInventoryItem)
                {
                    txtMessage.Text = $"I used the {selectedInventoryItem.Name} to unlock the {selectedRoomItem.Name}.";
                    selectedRoomItem.IsLocked = false;

                    // If unlocking reveals a hidden item
                    if (selectedRoomItem.HiddenItem != null)
                    {
                        txtMessage.Text += $"\nI found a {selectedRoomItem.HiddenItem.Name} inside!";
                        currentRoom.Items.Add(selectedRoomItem.HiddenItem);
                        selectedRoomItem.HiddenItem = null;
                    }

                    UpdateUI();
                }
                // Special case for the large key (winning condition)
                else if (selectedInventoryItem.Name == "large key")
                {
                    txtMessage.Text = "I used the large key and found a way out! I've escaped the room!";
                    MessageBox.Show("Congratulations! You've escaped the room!", "Victory", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (selectedRoomItem.IsLocked)
                {
                    txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.KeyDoesNotFit);
                }
                else
                {
                    txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.ItemNotUsable);
                }
            }
        }

        /// <summary>
        /// Drop an item from inventory back to the room
        /// </summary>
        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInventoryItem != null)
            {
                txtMessage.Text = $"I dropped the {selectedInventoryItem.Name}.";
                currentRoom.Items.Add(selectedInventoryItem);
                myItems.Remove(selectedInventoryItem);
                UpdateUI();
            }
        }

        private Door selectedDoor;

        private void DoorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoor = DoorListBox.SelectedItem as Door;
            if (selectedDoor != null)
            {
                OpenDoorButton.IsEnabled = selectedDoor.IsLocked;
                EnterRoomButton.IsEnabled = !selectedDoor.IsLocked;
            }
        }

        private void OpenDoorButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDoor != null && selectedInventoryItem != null)
            {
                if (selectedDoor.IsLocked && selectedDoor.Key == selectedInventoryItem)
                {
                    selectedDoor.IsLocked = false;
                    txtMessage.Text = $"I unlocked the {selectedDoor.Name} using the {selectedInventoryItem.Name}.";
                    UpdateUI();
                }
                else
                {
                    txtMessage.Text = RandomMessageGenerator.GetRandomMessage(MessageType.KeyDoesNotFit);
                }
            }
            else
            {
                txtMessage.Text = "You need to select a key from your inventory to unlock the door.";
            }
        }

        private void EnterRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDoor != null && !selectedDoor.IsLocked && selectedDoor.ToRoom != null)
            {
                currentRoom = selectedDoor.ToRoom;
                txtRoomDesc.Text = currentRoom.Description;
                txtMessage.Text = $"I walk through the {selectedDoor.Name}.";
                UpdateUI();
            }
            else if (selectedDoor?.ToRoom == null)
            {
                txtMessage.Text = "This door leads nowhere. Maybe it's the exit?";
            }
        }


        private void InitializeGame()
        {
            // Maak items
            Item bigKey = new Item { Name = "big key", Description = "A large old key." };
            Item chair = new Item { Name = "chair" };
            Item computer = new Item { Name = "computer" };
            Item screen = new Item { Name = "monitor" };

            // Maak kamers
            Room bedroom = new Room { Name = "bedroom", Description = "I seem to be in a medium sized bedroom..." };
            Room livingRoom = new Room { Name = "living room", Description = "A cozy living room with a sofa and TV." };
            Room computerRoom = new Room { Name = "computer room", Description = "A quiet room with a desk and computer." };

            // Items toevoegen aan kamers
            bedroom.Items.AddRange(new[] {
        new Item { Name = "poster" },
        new Item { Name = "floor mat" },
        new Item { Name = "bed" },
        new Item { Name = "locker", IsLocked = true, Key = bigKey, HiddenItem = new Item { Name = "note" } },
        chair,
        bigKey
    });

            livingRoom.Items.Add(new Item { Name = "remote" });

            computerRoom.Items.AddRange(new[] { computer, screen });

            // Maak deuren
            Door greenDoor = new Door { Name = "green door", IsLocked = true, Key = bigKey, Destination = livingRoom };
            Door toComputerRoom = new Door { Name = "hallway door", Destination = computerRoom };
            Door backToLiving = new Door { Name = "return door", Destination = livingRoom };
            Door lockedToNowhere = new Door { Name = "locked exit", IsLocked = true, Destination = null };

            // Deuren aan kamers koppelen
            bedroom.Doors.Add(greenDoor);
            livingRoom.Doors.Add(toComputerRoom);
            livingRoom.Doors.Add(lockedToNowhere);
            computerRoom.Doors.Add(backToLiving);

            // Stel startkamer en inventory in
            currentRoom = bedroom;
            inventory = new List<Item>();

            // UI updaten
            UpdateUI();
        }
    }
}