using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FESOVSE.Extension;
using Xceed.Wpf.Toolkit;
using Theme.WPF.Themes;
using System.Globalization;
using Fire_Emblem_Save_Tool;

namespace FESOVSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            initControls();
        }

        private void cbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UnitGrid1 != null && UnitGrid2 != null && ItemGrid1 != null && ItemGrid2 != null)
            {
                ComboBoxItem selectedModeItem = (ComboBoxItem)cbMode.SelectedItem;
                string selectedMode = selectedModeItem.Content.ToString();
                if (selectedMode == "Unit Editing")
                {
                    UnitGrid1.Visibility = Visibility.Visible;
                    UnitGrid2.Visibility = Visibility.Visible;
                    ItemGrid1.Visibility = Visibility.Collapsed;
                    ItemGrid2.Visibility = Visibility.Collapsed;
                }
                else if (selectedMode == "Convoy Editing")
                {
                    UnitGrid1.Visibility = Visibility.Collapsed;
                    UnitGrid2.Visibility = Visibility.Collapsed;
                    ItemGrid1.Visibility = Visibility.Visible;
                    ItemGrid2.Visibility = Visibility.Visible;
                }
            }
        }
        private void DecryptWorkData()
            {
                if (File.Exists(path))
                {
                    byte[] buffer;
                    byte[] buffer2;
                    int num;
                    using (FileStream stream2 = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(stream2))
                        {
                            byte[] buffer4 = new byte[0xd0];
                            reader.Read(buffer4, 0, 0xd0);
                            BitConverter.ToInt32(buffer4, 0);
                            buffer = new byte[0xc0];
                            Array.Copy(buffer4, 0, buffer, 0, 0xc0);
                            buffer2 = new byte[((int)stream2.Length) - 0xd0];
                            reader.Read(buffer2, 0, ((int)stream2.Length) - 0xd0);
                        }
                    }
                    byte[] source = new Huffman8().Decompress(buffer2);
                    List<byte> list1 = new List<byte>();
                    list1.AddRange(buffer.ToList<byte>());
                    list1.AddRange(source.ToList<byte>());
                    MemoryStream stream = new MemoryStream(list1.ToArray());
                    for (int i = 0; (num = stream.ReadByte()) != -1; i++)
                    {
                        WorkData = WorkData + $"{num:X2}" + " ";
                    }
                    ChapterHexData = WorkData;
                    WorkData = string.Empty;
                }
            }

            internal void EncryptWorkData()
            {
                byte[] buffer;
                byte[] buffer2;
                List<byte> list = new List<byte>();
                char[] separator = new char[] { ' ' };
                foreach (string str in WorkData.Split(separator))
                {
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        list.Add(byte.Parse(str, NumberStyles.HexNumber));
                    }
                }
                using (BinaryReader reader = new BinaryReader(new MemoryStream(list.ToArray())))
                {
                    byte[] buffer3 = new byte[0xd0];
                    reader.Read(buffer3, 0, 0xd0);
                    BitConverter.ToInt32(buffer3, 0);
                    buffer = new byte[0xc0];
                    Array.Copy(buffer3, 0, buffer, 0, 0xc0);
                    buffer2 = new byte[list.ToArray().Length - 0xc0];
                    Array.Copy(buffer3, 0xc0, buffer2, 0, 0x10);
                    reader.Read(buffer2, 0x10, list.ToArray().Length - 0xd0);
                }
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write(buffer, 0, buffer.Length);
                        writer.Write(0x434f4d50);
                        writer.Write(2);
                        writer.Write(buffer2.Length);
                        writer.Write(GetChecksum(GetChecksum(0, buffer), buffer2));
                        writer.Write(new Huffman8().Compress(buffer2));
                    }
                }
                WorkData = string.Empty;
            }

            private static uint GetChecksum(uint oldChecksum, byte[] data)
            {
                uint num = ~oldChecksum;
                uint length = (uint)data.Length;
                uint[] numArray = new uint[] {
                0, 0x77073096, 0xee0e612c, 0x990951ba, 0x76dc419, 0x706af48f, 0xe963a535, 0x9e6495a3, 0xedb8832, 0x79dcb8a4, 0xe0d5e91e, 0x97d2d988, 0x9b64c2b, 0x7eb17cbd, 0xe7b82d07, 0x90bf1d91,
                0x1db71064, 0x6ab020f2, 0xf3b97148, 0x84be41de, 0x1adad47d, 0x6ddde4eb, 0xf4d4b551, 0x83d385c7, 0x136c9856, 0x646ba8c0, 0xfd62f97a, 0x8a65c9ec, 0x14015c4f, 0x63066cd9, 0xfa0f3d63, 0x8d080df5,
                0x3b6e20c8, 0x4c69105e, 0xd56041e4, 0xa2677172, 0x3c03e4d1, 0x4b04d447, 0xd20d85fd, 0xa50ab56b, 0x35b5a8fa, 0x42b2986c, 0xdbbbc9d6, 0xacbcf940, 0x32d86ce3, 0x45df5c75, 0xdcd60dcf, 0xabd13d59,
                0x26d930ac, 0x51de003a, 0xc8d75180, 0xbfd06116, 0x21b4f4b5, 0x56b3c423, 0xcfba9599, 0xb8bda50f, 0x2802b89e, 0x5f058808, 0xc60cd9b2, 0xb10be924, 0x2f6f7c87, 0x58684c11, 0xc1611dab, 0xb6662d3d,
                0x76dc4190, 0x1db7106, 0x98d220bc, 0xefd5102a, 0x71b18589, 0x6b6b51f, 0x9fbfe4a5, 0xe8b8d433, 0x7807c9a2, 0xf00f934, 0x9609a88e, 0xe10e9818, 0x7f6a0dbb, 0x86d3d2d, 0x91646c97, 0xe6635c01,
                0x6b6b51f4, 0x1c6c6162, 0x856530d8, 0xf262004e, 0x6c0695ed, 0x1b01a57b, 0x8208f4c1, 0xf50fc457, 0x65b0d9c6, 0x12b7e950, 0x8bbeb8ea, 0xfcb9887c, 0x62dd1ddf, 0x15da2d49, 0x8cd37cf3, 0xfbd44c65,
                0x4db26158, 0x3ab551ce, 0xa3bc0074, 0xd4bb30e2, 0x4adfa541, 0x3dd895d7, 0xa4d1c46d, 0xd3d6f4fb, 0x4369e96a, 0x346ed9fc, 0xad678846, 0xda60b8d0, 0x44042d73, 0x33031de5, 0xaa0a4c5f, 0xdd0d7cc9,
                0x5005713c, 0x270241aa, 0xbe0b1010, 0xc90c2086, 0x5768b525, 0x206f85b3, 0xb966d409, 0xce61e49f, 0x5edef90e, 0x29d9c998, 0xb0d09822, 0xc7d7a8b4, 0x59b33d17, 0x2eb40d81, 0xb7bd5c3b, 0xc0ba6cad,
                0xedb88320, 0x9abfb3b6, 0x3b6e20c, 0x74b1d29a, 0xead54739, 0x9dd277af, 0x4db2615, 0x73dc1683, 0xe3630b12, 0x94643b84, 0xd6d6a3e, 0x7a6a5aa8, 0xe40ecf0b, 0x9309ff9d, 0xa00ae27, 0x7d079eb1,
                0xf00f9344, 0x8708a3d2, 0x1e01f268, 0x6906c2fe, 0xf762575d, 0x806567cb, 0x196c3671, 0x6e6b06e7, 0xfed41b76, 0x89d32be0, 0x10da7a5a, 0x67dd4acc, 0xf9b9df6f, 0x8ebeeff9, 0x17b7be43, 0x60b08ed5,
                0xd6d6a3e8, 0xa1d1937e, 0x38d8c2c4, 0x4fdff252, 0xd1bb67f1, 0xa6bc5767, 0x3fb506dd, 0x48b2364b, 0xd80d2bda, 0xaf0a1b4c, 0x36034af6, 0x41047a60, 0xdf60efc3, 0xa867df55, 0x316e8eef, 0x4669be79,
                0xcb61b38c, 0xbc66831a, 0x256fd2a0, 0x5268e236, 0xcc0c7795, 0xbb0b4703, 0x220216b9, 0x5505262f, 0xc5ba3bbe, 0xb2bd0b28, 0x2bb45a92, 0x5cb36a04, 0xc2d7ffa7, 0xb5d0cf31, 0x2cd99e8b, 0x5bdeae1d,
                0x9b64c2b0, 0xec63f226, 0x756aa39c, 0x26d930a, 0x9c0906a9, 0xeb0e363f, 0x72076785, 0x5005713, 0x95bf4a82, 0xe2b87a14, 0x7bb12bae, 0xcb61b38, 0x92d28e9b, 0xe5d5be0d, 0x7cdcefb7, 0xbdbdf21,
                0x86d3d2d4, 0xf1d4e242, 0x68ddb3f8, 0x1fda836e, 0x81be16cd, 0xf6b9265b, 0x6fb077e1, 0x18b74777, 0x88085ae6, 0xff0f6a70, 0x66063bca, 0x11010b5c, 0x8f659eff, 0xf862ae69, 0x616bffd3, 0x166ccf45,
                0xa00ae278, 0xd70dd2ee, 0x4e048354, 0x3903b3c2, 0xa7672661, 0xd06016f7, 0x4969474d, 0x3e6e77db, 0xaed16a4a, 0xd9d65adc, 0x40df0b66, 0x37d83bf0, 0xa9bcae53, 0xdebb9ec5, 0x47b2cf7f, 0x30b5ffe9,
                0xbdbdf21c, 0xcabac28a, 0x53b39330, 0x24b4a3a6, 0xbad03605, 0xcdd70693, 0x54de5729, 0x23d967bf, 0xb3667a2e, 0xc4614ab8, 0x5d681b02, 0x2a6f2b94, 0xb40bbe37, 0xc30c8ea1, 0x5a05df1b, 0x2d02ef8d
            };
                if (length > 0)
                {
                    int index = -1;
                    if ((length % 2) == 1)
                    {
                        index++;
                        num = numArray[(byte)(data[index] ^ (num & 0xff))] ^ (num >> 8);
                    }
                    uint num5 = length >> 1;
                    while (num5 > 0)
                    {
                        num5--;
                        int num6 = (byte)(data[index + 1] ^ (num & 0xff));
                        index += 2;
                        uint num4 = numArray[num6] ^ (num >> 8);
                        num = numArray[(byte)(data[index] ^ (num4 & 0xff))] ^ (num4 >> 8);
                    }
                }
                return ~num;
            }

            internal void LoadChapterData(string fileName)
            {
                path = fileName;
                DecryptWorkData();
                ChapterHexData = ChapterHexData.Replace(" ", "");
                byte[] ChapterHexDataBytes = new byte[ChapterHexData.Length / 2];
                for (int i = 0; i < ChapterHexDataBytes.Length; i++)
                {
                    string subStr = ChapterHexData.Substring(i * 2, 2);
                    ChapterHexDataBytes[i] = Convert.ToByte(subStr, 16);
                }
                _saveFile = ChapterHexDataBytes;
                loadUnits();
                loadConvoy();
                loadItems();
                loadClasses();
                bindEvents();
            }

            internal void SaveChapterData()
            {
                string _saveFileString = Convert.ToHexString(_saveFile);
                StringBuilder spacedHex = new StringBuilder();

                for (int i = 0; i < _saveFileString.Length; i += 2)
                {
                    spacedHex.Append(_saveFileString.Substring(i, 2));
                    if (i < _saveFileString.Length - 2)
                    {
                        spacedHex.Append(" ");
                    }
                }

                ChapterHexData = spacedHex.ToString();
                WorkData = ChapterHexData;
                EncryptWorkData();

            }

            public string ChapterHexData { get; set; }

            private string WorkData { get; set; }


            private byte[] _saveFile; //contents of the save file
            private string path; //location of the save file
            private IEnumerable<IntegerUpDown> upDwnBoxes; //list of numeric updownboxes contained in window

            private void ChangeTheme(object sender, RoutedEventArgs e)
            {
                switch (((MenuItem)sender).Uid)
                {
                    case "0":
                        ThemesController.SetTheme(ThemeType.DarkTheme);
                        break;
                    case "1":
                        ThemesController.SetTheme(ThemeType.LightTheme);
                        break;
                }

                e.Handled = true;
            }

            private void openFile_Click(object sender, RoutedEventArgs e)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Chapter Files|*"; //filter what shows in the open file dialog box
                ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory; //set starting directory
                if (ofd.ShowDialog() == true)
                {
                    path = ofd.FileName;
                    string fileName = ofd.FileName;
                    if (path.Contains("Chapter") && path.Contains("dec")) loadFile(); //check if file is the right file, load if true
                    else if (path.Contains("Chapter")) LoadChapterData(fileName);
                    else System.Windows.MessageBox.Show("Not a Chapter file");
                }

            }

            private void saveFile_Click(object sender, RoutedEventArgs e)
            {
                if (path.Contains("Chapter") && path.Contains("dec"))
                {
                    File.WriteAllBytes(path, _saveFile);
                    System.Windows.MessageBox.Show("File Saved Successfully");
                }
                else if (path.Contains("Chapter")) 
                {
                    SaveChapterData();
                    System.Windows.MessageBox.Show("File Saved Successfully");
                }
                else System.Windows.MessageBox.Show("No File Found");

            }


            #region Utility Functions

            /* checks if a sequence of bytes is in the save file */
            private int hasData(int blockSize, byte[] data, int start = 0)
            {
                // Check if start index is within bounds
                if (start < 0 || start >= _saveFile.Length)
                {
                    return -1; // Invalid start index
                }

                // Calculate max length
                int maxLength = start + blockSize;

                // Ensure maxLength does not exceed array length
                if (maxLength > _saveFile.Length)
                {
                    maxLength = _saveFile.Length;
                }

                // Loop through the search space
                for (int i = start; i + data.Length <= maxLength; i++)
                {
                    bool isSame = true;
                    // Loop through the data to check
                    for (int j = 0; j < data.Length; j++)
                    {
                        // Check if index is within bounds
                        if (i + j >= _saveFile.Length || _saveFile[i + j] != data[j])
                        {
                            isSame = false;
                            break;
                        }
                    }
                    if (isSame)
                    {
                        return i; // Return index if data is found
                    }
                }
                return -1; // Return -1 if data is not found within the search space
            }

            /* converts a string of hex into byte array */
            private byte[] hexToBytes(string hexStr)
            {
                byte[] byteVal = new byte[hexStr.Length / 2];
                for (int i = 0; i < byteVal.Length; i++)
                {
                    string subStr = hexStr.Substring(i * 2, 2);
                    byteVal[i] = Convert.ToByte(subStr, 16);
                }

                return byteVal;
            }

            /* return a hex string of a value at a specific address */
            private string getBytesValue(int startAddress, int size)
            {
                StringBuilder hex = new StringBuilder(size * 2);
                for (int i = 0; i < size; i++)
                {
                    var b = _saveFile[startAddress + i];
                    hex.AppendFormat("{0:X2}", b);
                }

                return hex.ToString();
            }

            /* find the location of the inventory of character */
            private int findItemAddress(int charIDStart)
            {
                byte[] itemByte = { 2, 1 }; //items are labelled with 02 01
                int charBlockSize = 184; //search space for a character block, char block changes depending on skills,supports etc
                                         //so this is just an assumable, if character's block is smaller it will get the next block's
                                         //works well if each character holds an item
                return hasData(charBlockSize, itemByte, charIDStart);
            }
            
            private int findConvoyItemAddress(int itemIDStart)
            {
                byte[] itemByte = { 2, 1 };
                int convoyBlockSize = 2800; // addresses are 14 bytes long x 200 total items between Alm and Celica
                return hasData(convoyBlockSize, itemByte, itemIDStart);
            }

            #endregion

            #region Setup Functions

            private void initControls()
            {
                cbItem.IsHitTestVisible = false; //disable controls at setup
                cbForge.IsHitTestVisible = false;
                cbClass.IsHitTestVisible = false;
                cnItem.IsHitTestVisible = false;
                cnForge.IsHitTestVisible = false;
                upDwnBoxes = this.FindVisualChildren<IntegerUpDown>();

            }
            /* reading file and loading database into controls*/
            private void loadFile()
            {
                _saveFile = File.ReadAllBytes(path);
                loadUnits();
                loadConvoy();
                loadItems();
                loadClasses();
                bindEvents();
            }

            private void loadUnits()
            {
                //getting the pointer to character stored at 0xCC
                int charBlockAddress = 0;
                for (int i = 0; i < 4; i++)
                {
                    charBlockAddress = (_saveFile[0xCC + i] << (i * 8)) | charBlockAddress;
                    //its just 2 bytes but I take 4 anyway
                }

                var charDB = new Data.CharacterDatabase(); //init database of characters from xml file
                var units = charDB.getAll(); //list of all units
                var currentUnits = new List<Data.Character>(); //units that are currently available in game
                foreach (Data.Character c in units)
                {
                    byte[] uID = hexToBytes(c.CharID);
                    int f = hasData(_saveFile.Length - charBlockAddress, uID, charBlockAddress); //check if character is available
                    if (f != -1)
                    {
                        c.StartAddress = f; //set character's start address for easy access (offset of start of character id)
                        c.ItemAddress = findItemAddress(f + 30); //also set the item address as its different depending on character (no specific offset)
                        currentUnits.Add(c);
                    }
                }
                //enabling data source for the control
                unitList.ItemsSource = currentUnits;
                unitList.DisplayMemberPath = "Name";
                unitList.SelectedValuePath = "ItemAddress";

            }

            private void loadConvoy() //about the same as loadUnits() right now
            {
                //getting the pointer to character stored at 0xCC
                int convoyBlockAddress = 0;
                for (int i = 0; i < 4; i++)
                {
                    convoyBlockAddress = (_saveFile[0xD0 + i] << (i * 8)) | convoyBlockAddress;
                    //its just 2 bytes but I take 4 anyway
                }

                var itemDB = new Data.ItemDatabase(); //init database of characters from xml file
                var items = itemDB.getAll(); //list of all units
                var currentItems = new List<Data.Item>(); //units that are currently available in game
                foreach (Data.Item c in items)
                {
                    byte[] iID = hexToBytes(c.ItemID);
                    int f = hasData(_saveFile.Length - convoyBlockAddress, iID, convoyBlockAddress); //check if character is available
                    if (f != -1)
                    {
                        c.ConvoyStartAddress = f; //set character's start address for easy access (offset of start of character id)
                        c.ConvoyItemAddress = findItemAddress(f + 30); //also set the item address as its different depending on character (no specific offset)
                        currentItems.Add(c);
                    }
                }
                //enabling data source for the control
                itemList.ItemsSource = currentItems;
                itemList.DisplayMemberPath = "Name";
                itemList.SelectedValuePath = "Hex";

            }

            private void loadItems()
            {
                var itemDB = new Data.ItemDatabase();
                var items = itemDB.getAll();
                cbItem.ItemsSource = items;
                cbItem.DisplayMemberPath = "Name";
                cbItem.SelectedValuePath = "Hex";
                cnItem.ItemsSource = items;
                cnItem.DisplayMemberPath = "Name";
                cnItem.SelectedValuePath = "Hex";
            }

            private void loadClasses()
            {
                var charClassDB = new Data.CharacterClassDatabase();
                var allClasses = charClassDB.getAll();
                cbClass.ItemsSource = allClasses;
                cbClass.DisplayMemberPath = "Name";
                cbClass.SelectedValuePath = "ClassID";

            }

        #endregion

            #region Updating Panel Functions

        /* updating the window screen */
        private void updateDescription(object sender, SelectionChangedEventArgs e)
        {
            unBindEvents();

            cbItem.IsHitTestVisible = true; //enable control
            cnItem.IsHitTestVisible=true;
            var character = (Data.Character)unitList.SelectedItem; //get the currently selected character
            var item = (Data.Item)itemList.SelectedItem;

            if (character != null) // check if character is not null
            {
                if (character.ItemAddress != -1) //if character is holding an item
                {
                    string itemHex = getBytesValue(character.ItemAddress + 6, 8);
                    cbItem.SelectedValue = itemHex; //update the item combo box with unit's item
                    if (cbItem.SelectedValue == null) //disable combobox if item is not in database
                    {                                 //will remove once resource is complete
                        cbItem.IsHitTestVisible = false;
                        cbForge.IsHitTestVisible = false;
                    }
                    else
                    {
                        var currentItem = (Data.Item)cbItem.SelectedItem;
                        int currentForge = _saveFile[character.ItemAddress + 5] >> 4; // forge offset +5 after 02 01
                        updateForgeBox(currentItem.MaxForges, currentForge);
                    }
                }
                else
                {
                    //disable forge and item comboboxes if no held item
                    cbItem.SelectedIndex = -1;
                    cbForge.SelectedIndex = -1;
                    cbItem.IsHitTestVisible = false;
                    cbForge.IsHitTestVisible = false;
                }

                updateStatBox();
                updateClassBox();
                bindEvents();

            }
            if (item != null) 
            {
                if (item.ConvoyItemAddress != -1)
                {
                    string itemHex = getBytesValue(item.ConvoyItemAddress + 6, 8);
                    cnItem.SelectedValue = itemHex;
                    if (cnItem.SelectedValue == null) //disable combobox if item is not in database
                    {                                 //will remove once resource is complete
                        cnItem.IsHitTestVisible = false;
                        cnForge.IsHitTestVisible = false;
                    }
                    else
                    {
                        var currentItem = (Data.Item)cnItem.SelectedItem;
                        int currentForge = _saveFile[item.ConvoyItemAddress + 5] >> 4; // forge offset +5 after 02 01
                        updateForgeBox(currentItem.MaxForges, currentForge);
                    }
                }
                else
                {
                    //disable forge and item comboboxes if no held item
                    cnItem.SelectedIndex = -1;
                    cnForge.SelectedIndex = -1;
                    cnItem.IsHitTestVisible = false;
                    cnForge.IsHitTestVisible = false;
                }

                    bindEvents();
            }
        
        }
        /* update the forge combo box based on current item in item combo box*/
            private void updateForgeBox(int maxForges, int currentForge = 0)
            {
            cbForge.Items.Clear();
            cnForge.Items.Clear();
            if (maxForges != 0)
            {
                for (int i = 0; i <= maxForges; i++)
                {
                    cbForge.Items.Add(i);
                    cnForge.Items.Add(i);
                }
                cbForge.IsHitTestVisible = true;
                cbForge.SelectedIndex = currentForge;
                cnForge.IsHitTestVisible = true;
                cnForge.SelectedIndex = currentForge;
            }
            else
            {
                cbForge.IsHitTestVisible = false;
                cnForge.IsHitTestVisible = false;
            }
            }
            /* updates the numeric updowns based on character's current stat*/
            private void updateStatBox()
            {
                Data.Character character = (Data.Character)unitList.SelectedItem;
                int level = _saveFile[character.StartAddress - 2]; //level offset 2 bytes before character id
                int exp = _saveFile[character.StartAddress - 1]; //exp offset 1 byte before character id
                int statAddress = character.StartAddress + 21; //character stats offset 21 bytes after character id
                int counter = -2;
                foreach (IntegerUpDown iUD in upDwnBoxes)
                {
                    if (counter == -2) iUD.Value = level;
                    else if (counter == -1) iUD.Value = exp;
                    else
                    {
                        //calculates stat using character base and value from save file
                        //character stats = value from save file + base stats
                        iUD.Minimum = character.BaseStats[counter];
                        iUD.Maximum = character.MaxStats[counter];
                        iUD.Value = _saveFile[statAddress + counter] + character.BaseStats[counter];
                    }
                    counter++;
                }
            }
            /* updates class box based on selected character*/
            private void updateClassBox()
            {
                cbClass.IsHitTestVisible = true;
                Data.Character character = (Data.Character)unitList.SelectedItem;
                string classHex = getBytesValue(character.StartAddress + 8, 8);
                cbClass.SelectedValue = classHex;
            }

            #endregion

            #region Event Functions
            /* fired when item combo box is changed*/
            private void itemBoxChanged(object sender, EventArgs e)
            {
                unBindEvents();

                Data.Character character = (Data.Character)unitList.SelectedItem;
                Data.Item currentItem = (Data.Item)cbItem.SelectedItem;
                byte[] itemHex = hexToBytes(currentItem.Hex);
                byte[] itemID = hexToBytes(currentItem.ItemID);
                byte[] itemMiddleHex;
                if (currentItem.isDLC) itemMiddleHex = hexToBytes("010008"); //I see this pattern if its a dlc
                else itemMiddleHex = hexToBytes("000000"); //default value, no forges, etc.
                IEnumerable<byte> itemVal = itemID.Concat(itemMiddleHex).Concat(itemHex); //combine the bytes to form 12 bytes of item value
                int start = character.ItemAddress + 2;
                foreach (byte b in itemVal)
                {
                    _saveFile[start] = b; //insert the new value in file
                    start++;
                }

                updateForgeBox(currentItem.MaxForges); //disable/enable forge box depending on current item

                bindEvents();
            }
        /* fired when forge combo box changed */
        private void forgeBoxChanged(object sender, EventArgs e)
        {
            unBindEvents();

            Data.Character character = (Data.Character)unitList.SelectedItem;
            if (cbForge.SelectedItem != null)
            {
                byte forgeVal = Convert.ToByte(cbForge.SelectedItem.ToString(), 16); //get the value changed
                byte val = _saveFile[character.ItemAddress + 5]; //forge offset 5 after 02 01, the leftmost 4 bits only
                val = (byte)(val & 0x0F); //clear leftmost 4 bits
                val = (byte)(val | (forgeVal << 4)); //add the new value to leftmost 4 bits
                _saveFile[character.ItemAddress + 5] = val;

                bindEvents();
            }
        }

            private void statChanged(object sender, EventArgs e)
            {
                unBindEvents();

                Data.Character character = (Data.Character)unitList.SelectedItem;
                IntegerUpDown statBox = (IntegerUpDown)sender; //get the control that fired the event
                Enum.Stat stat = XMLProperties.StatProperty.GetStat(statBox); //get value of attached property

                if (stat == Enum.Stat.Level || stat == Enum.Stat.Experience)
                {
                    _saveFile[character.StartAddress + (int)stat] = (byte)statBox.Value; //update exp or level based on current value
                }
                else
                {
                    // new value is the character base stats subtracted from current value
                    // because the value in save file is just the values to be added to the base stat
                    byte newVal = (byte)(statBox.Value - character.BaseStats[(int)stat]);
                    _saveFile[character.StartAddress + 21 + (int)stat] = newVal;
                }

                bindEvents();
            }

            private void classChanged(object sender, EventArgs e)
            {
                unBindEvents();

                Data.Character character = (Data.Character)unitList.SelectedItem;
                Data.CharacterClass cc = (Data.CharacterClass)cbClass.SelectedItem;
                byte[] classVal = hexToBytes(cc.ClassID);
                int index = character.StartAddress + 8;
                foreach (byte b in classVal)
                {
                    _saveFile[index] = b; //insert new value of class into save file
                    index++;
                }

                bindEvents();
            }


            private void unBindEvents()
            {
                cbItem.SelectionChanged -= itemBoxChanged;
                unitList.SelectionChanged -= updateDescription;
                cbForge.SelectionChanged -= forgeBoxChanged;
                cbClass.SelectionChanged -= classChanged;
                cnItem.SelectionChanged -= itemBoxChanged;
                itemList.SelectionChanged -= updateDescription;
                cnForge.SelectionChanged -= forgeBoxChanged;
                foreach (IntegerUpDown x in upDwnBoxes)
                {
                    x.ValueChanged -= statChanged;
                }
            }

            private void bindEvents()
            {
                cbItem.SelectionChanged += itemBoxChanged;
                unitList.SelectionChanged += updateDescription;
                cbForge.SelectionChanged += forgeBoxChanged;
                cbClass.SelectionChanged += classChanged;
                cnItem.SelectionChanged += itemBoxChanged;
                itemList.SelectionChanged += updateDescription;
                cnForge.SelectionChanged += forgeBoxChanged;
                foreach (IntegerUpDown x in upDwnBoxes)
                {
                    x.ValueChanged += statChanged;
                }
            }

            #endregion

        }
}
