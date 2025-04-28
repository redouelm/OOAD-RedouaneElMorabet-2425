using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public static class RandomMessageGenerator
    {
        private static readonly string[] itemNotPortableMessages = new string[]
       {
            "I can't pick that up. It's too heavy or fixed in place.",
            "That's not something I can carry around.",
            "No way I'm picking that up. It won't budge!"
       };

        private static readonly string[] keyDoesNotFitMessages = new string[]
        {
            "This key doesn't seem to fit.",
            "Wrong key for this lock.",
            "The key and lock don't match. I need to find the right one."
        };

        private static readonly string[] itemNotUsableMessages = new string[]
        {
            "That doesn't seem to work.",
            "I don't think these items can be used together.",
            "Nothing happens when I try to use these together."
        };

        private static readonly Random random = new Random();

        public static string GetRandomMessage(MessageType type)
        {
            switch (type)
            {
                case MessageType.ItemNotPortable:
                    return itemNotPortableMessages[random.Next(itemNotPortableMessages.Length)];
                case MessageType.KeyDoesNotFit:
                    return keyDoesNotFitMessages[random.Next(keyDoesNotFitMessages.Length)];
                case MessageType.ItemNotUsable:
                    return itemNotUsableMessages[random.Next(itemNotUsableMessages.Length)];
                default:
                    return "Unknown message type";
            }
        }
    }
}