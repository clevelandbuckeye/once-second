using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace HubApp1.Data
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    [DataContract]
    public class SampleDataItem
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
        }

        [DataMember]
        public string UniqueId { get; private set; }
        [DataMember]
        public string Title { get; private set; }
        [DataMember]
        public string Subtitle { get;  set; }
        [DataMember]
        public string Description { get;  set; }
        [DataMember]
        public string ImagePath { get; private set; }
        [DataMember]
        public string Content { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    [DataContract]
    public class SampleDataGroup
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<SampleDataItem>();
        }
        [DataMember]
        public string UniqueId { get; private set; }
        [DataMember]
        public string Title { get; private set; }
        [DataMember]
        public string Subtitle { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public string ImagePath { get; private set; }
        [DataMember]
        public ObservableCollection<SampleDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// 
    /// SampleDataSource initializes with data read from a static json file included in the 
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    [DataContract]
    public sealed class SampleDataSource
    {
        public static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _groups = new ObservableCollection<SampleDataGroup>();
        [DataMember]
        public ObservableCollection<SampleDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<SampleDataGroup>> GetGroupsAsync()
        {
            await _sampleDataSource.GetSampleDataAsync();

            return _sampleDataSource.Groups;
        }

        public static async Task<SampleDataGroup> GetGroupAsync(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<SampleDataItem> GetItemAsync(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetSampleDataAsync()
        {
            if (this._groups.Count != 0)
                return;

            await GetImagesFromFolder();

            //await SampleJasonGet();
        }

        public async Task GetImagesFromFolder()
        {
            //SampleDataGroup group = await getGroupFromPicturesLibrary();

            //SampleDataGroup group = await getRomingGromeup();
            await SampleJasonGet();

            //this.Groups.Add(group);
            //await JSONSerialize(this);
        }
        public async Task SaveGroups()
        {
            //using (FileStream fs = File.Open(@"c:\person.json", FileMode.CreateNew))
            //using (StreamWriter sw = new StreamWriter(fs))
            //using (JsonWriter jw = new JsonTextWriter(sw))
            //{
            //    jw.Formatting = Formatting.Indented;

            //    JsonSerializer serializer = new JsonSerializer();
            //    serializer.Serialize(jw, person);
            //}
            //this.ser
        }
        public async Task JSONSerialize(SampleDataSource objStudent)

        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(SampleDataSource));
            jsonSer.WriteObject(stream, objStudent);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            //StorageFile.CreateStreamedFileAsync("databaseaa.json", )
            string jsonText = sr.ReadToEnd();
            var theNextFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync("database11.json", CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(theNextFile, jsonText);
            //theNextFile.
        }

        //public async Task SaveitWhateverQuick()
        //{
        //    //this.
        //}

        private static async Task<SampleDataGroup> getRomingGromeup()
        {
            var picfold = ApplicationData.Current.RoamingFolder;
            SampleDataGroup group = new SampleDataGroup("1", "unchart", "undescript", "1.png", "the unfiltered list or starting list");
            foreach (StorageFile picfil in await picfold.GetFilesAsync())
            {
                //var newfi = await picfil.CopyAsync(ApplicationData.Current.RoamingFolder);
                SampleDataItem rer = new SampleDataItem(picfil.Name,
                                                        picfil.ContentType,
                                                        picfil.DateCreated.ToString(),
                                                        picfil.Path,
                                                        picfil.FileType,
                                                        picfil.FolderRelativeId);


                group.Items.Add(rer);
            }

            return group;
        }

        private static async Task<SampleDataGroup> getGroupFromPicturesLibrary()
        {
            var picfold = KnownFolders.PicturesLibrary;
            SampleDataGroup group = new SampleDataGroup("1", "unchart", "undescript", "1.png", "the unfiltered list or starting list");
            foreach (StorageFile picfil in await picfold.GetFilesAsync())
            {
                var newfi = await picfil.CopyAsync(ApplicationData.Current.RoamingFolder);
                SampleDataItem rer = new SampleDataItem(newfi.Name,
                                                        newfi.ContentType,
                                                        newfi.DateCreated.ToString(),
                                                        newfi.Path,
                                                        newfi.FileType,
                                                        newfi.FolderRelativeId);

                group.Items.Add(rer);
            }

            return group;
        }
        public async Task<Uri> getStorageUri()
        {
            var wereer = await ApplicationData.Current.RoamingFolder.GetFileAsync("database11.json");
            return new Uri(wereer.Path);  //uri generation C# next 6.0
        }
        private async Task SampleJasonGet()
        {
            //Uri dataUri = await getStorageUri();// new Uri("ms-appx:///DataModel/SampleData.json");

            //var sdfefe = ApplicationData.Current.RoamingFolder.GetFileAsync("database11.json")

            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var file = await ApplicationData.Current.RoamingFolder.GetFileAsync("database11.json");

            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                SampleDataGroup group = new SampleDataGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Title"].GetString(),
                                                            groupObject["Subtitle"].GetString(),
                                                            groupObject["ImagePath"].GetString(),
                                                            groupObject["Description"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new SampleDataItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["Subtitle"].GetString(),
                                                       itemObject["ImagePath"].GetString(),
                                                       itemObject["Description"].GetString(),
                                                       itemObject["Content"].GetString()));
                }
                this.Groups.Add(group);
            }
        }
    }
}