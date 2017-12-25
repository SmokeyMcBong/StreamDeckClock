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
                    //Get the current time (hours, minutes and am/pm)
                    string time_output_hours = DateTime.Now.ToString("hh");
                    string time_output_minutes = DateTime.Now.ToString("mm");
                    string time_output_ampm = DateTime.Now.ToString("tt");
                    
                    //Check if hours string is not empty
                    if (!string.IsNullOrEmpty(time_output_hours))
                    {
                        //check if hours start with "0"
                        if (time_output_hours.StartsWith("0"))
                        {
                            //remove starting "0" from string
                            time_output_hours = time_output_hours.Remove(0, 1);
                        }
                        //send results to Set_Time to deal with
                        Set_Time(time_output_hours, key_location_hours);
                    }

                    //Check if minutes string is not empty
                    if (!string.IsNullOrEmpty(time_output_minutes))
                    {
                        //send results to Set_Time to deal with
                        Set_Time(time_output_minutes, key_location_minutes);
                    }

                    //Check if am/pm string is not empty
                    if (!string.IsNullOrEmpty(time_output_ampm))
                    {
                        //send results to Set_Time to deal with
                        Set_Time(time_output_ampm, key_location_ampm);
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
