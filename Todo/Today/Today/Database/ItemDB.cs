using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Today.Model;

namespace Today.Services
{
    public partial class ItemDB
    {
        static ItemDB defaultInstance = new ItemDB();
       
        private MobileServiceClient client;
        private IMobileServiceTable<Item> Module2Table1;


        private ItemDB()
        {
            this.client = new MobileServiceClient("http://module2njapp.azurewebsites.net");
            this.Module2Table1 = client.GetTable<Item>();
        }

        public static ItemDB DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public async Task<ObservableCollection<Item>> GetItemsAsync(bool syncItems = false)
        {
            try
            {
                IEnumerable<Item> items = await Module2Table1.OrderBy(Item => Item.Name).ToEnumerableAsync();
                return new ObservableCollection<Item>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveAsync(Item item)
        {
            if (item.Id == null)
            {
                Debug.WriteLine(@"Sync error: {0}", item.Id);
                Debug.WriteLine(@"Sync error: {0}", item.Name);
                Debug.WriteLine(@"Sync error: {0}", "Insert");
                try
                {
                    await Module2Table1.InsertAsync(item);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(@"Sync error: {0}", e.Message);
                }


                Debug.WriteLine(@"Sync error: {0}", "Inserted");
            }
            else
            {
                Debug.WriteLine(@"Sync error: {0}", "Update");
                await Module2Table1.UpdateAsync(item);
                Debug.WriteLine(@"Sync error: {0}", "Updated");
            }
        }


        public async Task DeleteAsync(Item item)
        {
                await Module2Table1.DeleteAsync(item);
        }
    }
}
