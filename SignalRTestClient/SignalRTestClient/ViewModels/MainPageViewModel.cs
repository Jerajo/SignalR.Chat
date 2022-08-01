using Microsoft.AspNetCore.SignalR.Client;
using SignalRTestClient.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SignalRTestClient.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private HubConnection HubConnection;
        private readonly string userInput;

        public MainPageViewModel()
        {
            Messages = new ObservableCollection<string>();
            SendMessage = new Command(OnSendMessage, () => IsConnected);
            Connect = new Command(OnConnect, () => !IsConnected);
            userInput = (Application.Current as App)?.Information.Name;
            IsConnected = false;
            OnInit();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<string> Messages { get; }
        public bool IsConnected { get; set; }
        public Command SendMessage { get; }
        public Command Connect { get; }
        public string MessageInput { get; set; }

        private async void OnInit()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/chathub")
                .WithAutomaticReconnect()
                .Build();

            HubConnection.Reconnecting += OnReconnecting;
            HubConnection.Reconnected += OnReconnected;
            HubConnection.Closed += OnClosed;
        }

        private void OnReceiveMessage(string user, Message message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var formattedMessage = $"{user}: {message.Text} | {message.Time}";
                Messages.Add(formattedMessage);
            });
        }

        private Task OnClosed(Exception arg)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string message = "The server connection is closed.";
                Messages.Add(message);
                IsConnected = false;
                SendMessage.ChangeCanExecute();
                Connect.ChangeCanExecute();
            });

            return Task.CompletedTask;
        }

        private Task OnReconnected(string arg)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string message = "The server is connected.";
                Messages.Add(message);
                IsConnected = true;
                SendMessage.ChangeCanExecute();
                Connect.ChangeCanExecute();
            });

            return Task.CompletedTask;
        }

        private Task OnReconnecting(Exception arg)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string message = "The server is trying to reconnect...";
                Messages.Add(message);
            });

            return Task.CompletedTask;
        }

        private async void OnSendMessage()
        {
            if (HubConnection != null)
                await HubConnection.SendAsync("SendMessage", userInput, new Message { Text = MessageInput });
        }

        private async void OnConnect()
        {
            HubConnection.On<string, Message>("ReceiveMessage", OnReceiveMessage);
            await HubConnection.StartAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string message = "The server is connected.";
                Messages.Add(message);
                IsConnected = true;
                SendMessage.ChangeCanExecute();
                Connect.ChangeCanExecute();
            });
        }
    }
}
