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
                        //send results to Set_Time to deal with
                        Set_Time(clock_hours, key_location_hours);
                    }

                    //Check if minutes string is not empty
                    if (!string.IsNullOrEmpty(clock_minutes))
                    {
                        //send results to Set_Time to deal with
                        Set_Time(clock_minutes, key_location_minutes);
                    }

                    //Check if am/pm string is not empty
                    if (!string.IsNullOrEmpty(clock_ampm))
                    {
                        //send results to Set_Time to deal with
                        Set_Time(clock_ampm, key_location_ampm);
                    }

                    //Wait 1 second before restarting loop
                    System.Threading.Thread.Sleep(1000);

                    //Check for key presses, if pressed send exit command 
                    deck.KeyPressed += Deck_KeyPressed;
                }

                void Set_Time(string time_result, int key_location)
                {
                    //Set corresponding icons to the time of day
                    string time_result_icon = time_result + ".png";
                    var clock_bitmap = StreamDeckKeyBitmap.FromFile("clock_icons\\" + time_result_icon);
                    deck.SetKeyBitmap(key_location, clock_bitmap);
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
