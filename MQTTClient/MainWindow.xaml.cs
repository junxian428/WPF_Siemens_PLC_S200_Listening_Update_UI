using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Windows;
using System.Threading.Tasks;
using S7.Net;

namespace MQTTClient
{
    public partial class MainWindow : Window
    {
        private Plc plc;
        private bool isListening;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartListeningButton_Click(object sender, RoutedEventArgs e)
        {
            if (plc == null)
            {
                // Create an instance of the S7.Net Plc object
                plc = new Plc(CpuType.S7200, "192.168.0.25", 0, 1);

                // Open the connection to the PLC
                plc.Open();
            }

            isListening = true;

            await Task.Run(async () =>
            {
                while (isListening)
                {
                    // Read the Q0.0 output
                    bool outputStatus = (bool)plc.Read("Q0.0");

                    // Update UI or perform any actions based on the output status
                    UpdateUI(outputStatus);

                    // Delay before the next read
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            });
        }

        private void StopListeningButton_Click(object sender, RoutedEventArgs e)
        {
            isListening = false;

            if (plc != null)
            {
                // Close the connection to the PLC
                plc.Close();
                plc = null;
            }
        }

        private void UpdateUI(bool outputStatus)
        {
            // Update UI elements or perform actions based on the PLC output status
            Dispatcher.Invoke(() =>
            {
                if (outputStatus)
                {
                    StatusTextBlock.Text = "Current Status: Open"; // Update the text block to show "Open"
                }
                else
                {
                    StatusTextBlock.Text = "Current Status: Close"; // Update the text block to show "Close"
                }
            });
        }

    }
}
