using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Today.Model
{
    public class Item
    {
        string id;
        string todo;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "todo")]
        public string Name
        {
            get { return todo; }
            set { todo = value; }
        }

        [Version]
        public string Version { get; set; }

    }
}
