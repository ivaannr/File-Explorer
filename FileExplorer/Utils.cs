using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FileExplorer.Model;

namespace FileExplorer
{
    internal static class Utils
    {

        public static string CastToCorrectSize(long size) {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            return size switch
            {
                < KB => $"{size} bytes",
                < MB => $"{size / KB} KB",
                < GB => $"{size / MB} MB",
                _ => $"{size / GB} GB"
            };
        }

        public static bool CanAccessDirectory(string path)
        {
            try
            {
                Directory.GetFiles(path);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo al directorio: {ex.Message}");
                return false;
            }
        }


        public static long CalculateDirectorySize(string folderPath)
        {
            long size = 0;
            try
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    try
                    {
                        FileInfo info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch(Exception ex) { Console.WriteLine(ex.Message); }
                }

                foreach (string dir in Directory.GetDirectories(folderPath)) {
                    size += CalculateDirectorySize(dir);
                }
            }
            catch (Exception ex) { Console.WriteLine("ERROR: " + ex.Message); }
            return size;
        }

        public static String[] ReadAllLinesFromFile(String path) 
            => File.Exists(path) ? File.ReadAllLines(path) : throw new FileNotFoundException($"File {path} not found.");

        public async static Task<List<string>> GetFavoriteDirectories() {
            try
            {
                return await Task.Run(() =>
                {
                    List<string> directories = new List<string>();
                    var lines = ReadAllLinesFromFile("favs.csv");

                    foreach (var line in lines)
                    {
                        var parts = line.Split(";");
                        String ID = parts[0];
                        String path = parts[1];

                        if (!Directory.Exists(path))
                        {
                            throw new DirectoryNotFoundException($"Directory {path} not found.");
                        }

                        directories.Add(line);
                    }

                    return directories;
                });
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }

        public static async Task<List<FavoriteDirectory>> ParseCSVData(List<String> directories) {
            try {
                return await Task.Run(() => {
                    List<FavoriteDirectory> favoriteDirectories = new List<FavoriteDirectory>();

                    foreach (String dir in directories) {

                        var parts = dir.Split(";");
                        String ID = parts[0];
                        String path = parts[1];

                        favoriteDirectories.Add(new FavoriteDirectory(ID, path));
                    }
                    return favoriteDirectories;
                });
            } catch (FileNotFoundException ex) {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex) {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return null;
        }



    }
}
