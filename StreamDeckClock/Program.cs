using System;
using System.Text;

using StreamDeckSharp;

namespace StreamDeckClock
{
    class Program
    {
        static void Main(string[] args)
        {
            /////////////////////////
            //   -key locations-   //
            //    4  3  2  1  0    //
            //    9  8  7  6  5    //
            //   14 13 12 11 10    //
            /////////////////////////

            //Set key locations
            var key_location_hours = 8;
            var key_location_minutes = 7;
            var key_location_ampm = 6;

            //Attempt to initially give the buttom time to change the image before starting the loop to avoid display lag
            System.Threading.Thread.Sleep(500);

            //Open the Stream Deck device
            using (var deck = StreamDeck.FromHID())
            {
                //Set loop
                while (true)
                {
                    //Get current time - Hour
                    string clock_hours = DateTime.Now.ToString("hh:mm:ss tt");
                    StringBuilder sb_clock_hours = new StringBuilder(clock_hours);
                    sb_clock_hours.Remove(2, 9);
                    clock_hours = sb_clock_hours.ToString();

                    //Get current time - Minutes
                    string clock_minutes = DateTime.Now.ToString("hh:mm:ss tt");
                    StringBuilder sb_clock_minutes = new StringBuilder(clock_minutes);
                    sb_clock_minutes.Remove(0, 3);
                    sb_clock_minutes.Remove(2, 6);
                    clock_minutes = sb_clock_minutes.ToString();

                    //Get current time - Am/Pm
                    string clock_ampm = DateTime.Now.ToString("hh:mm:ss tt");
                    StringBuilder sb_clock_ampm = new StringBuilder(clock_ampm);
                    sb_clock_ampm.Remove(0, 9);
                    clock_ampm = sb_clock_ampm.ToString();

                    //Check if hours string is not empty
                    if (!string.IsNullOrEmpty(clock_hours))
                    {
                        //check if hours start with "0"
                        if (clock_hours.StartsWith("0"))
                        {
                            //remove starting "0" from string
                            clock_hours = clock_hours.Remove(0, 1);
                        }
                        //Set corresponding icon to the hour of day
                        string clock_hours_icon = clock_hours + ".png";
                        var clock_hours_bitmap = StreamDeckKeyBitmap.FromFile("clock_icons\\" + clock_hours_icon);
                        deck.SetKeyBitmap(key_location_hours, clock_hours_bitmap);
                    }

                    //Check if minutes string is not empty
                    if (!string.IsNullOrEmpty(clock_minutes))
                    {
                        //Set corresponding icon to the minutes of day
                        string clock_minutes_icon = clock_minutes + ".png";
                        var clock_minutes_bitmap = StreamDeckKeyBitmap.FromFile("clock_icons\\" + clock_minutes_icon);
                        deck.SetKeyBitmap(key_location_minutes, clock_minutes_bitmap);
                    }

                    //Check if am/pm string is not empty
                    if (!string.IsNullOrEmpty(clock_ampm))
                    {
                        //Set corresponding icon to am/pm
                        string clock_ampm_icon = clock_ampm.ToLower() + ".png";
                        var clock_ampm_bitmap = StreamDeckKeyBitmap.FromFile("clock_icons\\" + clock_ampm_icon);
                        deck.SetKeyBitmap(key_location_ampm, clock_ampm_bitmap);
                    }
                    //Wait 1 second before restarting loop
                    System.Threading.Thread.Sleep(1000);

                    //Check for key presses, if pressed send exit command 
                    deck.KeyPressed += Deck_KeyPressed;
                }

                void Deck_KeyPressed(object sender, StreamDeckKeyEventArgs e)
                {
                    //Kill console App
                    Environment.Exit(0);
                }
            }
        }
    }
}
