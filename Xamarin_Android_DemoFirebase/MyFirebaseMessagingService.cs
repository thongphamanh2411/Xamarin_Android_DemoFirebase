using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;

namespace Xamarin_Android_DemoFirebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        // private string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            SendNotification(message.GetNotification().Title, message.GetNotification().Body, message.Data);

            var powerManager = (PowerManager)GetSystemService(PowerService);
            var wakeLock = powerManager.NewWakeLock(WakeLockFlags.ScreenDim | WakeLockFlags.AcquireCausesWakeup, "Demo");            
            wakeLock.Acquire();
            wakeLock.Release();

           

            //IntentFilter filter = new IntentFilter(Intent.ActionScreenOn);
            //filter.AddAction(Intent.ActionScreenOn);

        }
        private void SendNotification(string messageTitle, string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity)); intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.OneShot);
            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                                                            .SetSmallIcon(Resource.Mipmap.ic_launcher)
                                                            .SetContentTitle(messageTitle)
                                                            .SetContentText(messageBody).SetAutoCancel(true)
                                                            .SetContentIntent(pendingIntent)
                                                            .SetFullScreenIntent(pendingIntent, true);

            notificationBuilder.SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.icon_sfa));
            notificationBuilder.SetColor(Android.Graphics.Color.Rgb(33, 150, 243));


            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());


        }
    }
}