﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectSoftware.Facebook.Messenger.AspNetCore.Webhook;
using ReflectSoftware.Facebook.Messenger.Client;
using ReflectSoftware.Facebook.Messenger.Common.Models.Client;
using ReflectSoftware.Facebook.Messenger.Common.Models.Webhooks;
using ReflectSoftware.Insight;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiSample.AspNetCore.Controllers
{
    // https://developers.facebook.com/docs/messenger-platform

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("v1/inbound/customer/[controller]")]
    public class MessengerController : Controller
    {
        private static readonly HashSet<string> _FirstTimeCaller;

        private readonly WebhookHandler _webHookHandler;
        private readonly ClientMessenger _clientMessenger;

        /// <summary>
        /// Initializes the <see cref="MessengerController"/> class.
        /// </summary>
        static MessengerController()
        {
            _FirstTimeCaller = new HashSet<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessengerController"/> class.
        /// </summary>
        public MessengerController()
        {
            _webHookHandler = new WebhookHandler("channelsis", "79faa9a8710333289b595925e5fb7e72");            
            _clientMessenger = new ClientMessenger("EAAa2PqNTZABwBAAXOwwbAbZCaUFqU3KTeiZC1LCJjOr2ZAZB32bXx0p9gMqWedLJsse4xW8BXMatyYrbvIp0ICLiDmRVxM8Yp6cHPZATYjDsz0qZCmhhZAHJNeYpkVNMuEKjIw9goQUHgI54YOouZBzJa80fZCYoxtgJaWVYyQEKOhawZDZD");            
        }

        /// <summary>
        /// Receives the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ReceiveAsync(string id)
        {
            var result = await FacebookAsync();
            return result ?? BadRequest();
        }

        #region Facebook

        /// <summary>
        /// Handles the messaging asynchronous.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        private async Task HandleMessagingAsync(List<Messaging> messages)
        {
            foreach (var messaging in messages)
            {
                if (messaging.Message != null && !messaging.Message.IsEcho)
                {
                    /// User Send Message
                    /// This callback will occur when a message has been sent to your page.You may receive text messages or messages 
                    /// with attachments(image, audio, video, file or location).Callbacks contain a seq number which can be used 
                    /// to know the sequence of a message in a conversation. Messages are always sent in order.
                    /// You can subscribe to this callback by selecting the message field when setting up your webhook.

                    var userProfile = await _clientMessenger.GetUserProfileAsync(messaging.Sender.Id);
                    RILogManager.Default.SendJSON("userProfile", userProfile);

                    var result = await _clientMessenger.SendMessageAsync(messaging.Sender.Id, new TextMessage
                    {
                        Text = $"Hi, {userProfile.Firstname}. An agent will respond to your question shortly."
                    });

                    RILogManager.Default.SendJSON("Results", new[] { result });
                }
                else if (messaging.Postback != null)
                {
                }
                else if (messaging.Delivery != null)
                {
                    /// This callback will occur when a message a page has sent has been delivered.
                    /// You can subscribe to this callback by selecting the message_deliveries field when setting up your webhook.
                }
                else if (messaging.Read != null)
                {
                    /// This callback will occur when a message a page has sent has been read by the user.
                    /// You can subscribe to this callback by selecting the message_reads field when setting up your webhook.
                }
                else if (messaging.Optin != null)
                {
                    /// User Call "Message Us" 
                }
                else if (messaging.Referral != null)
                {
                    /// Referral
                }
                else if (messaging.AccountLinking != null)
                {
                    /// Account Linking
                }
            }
        }

        /// <summary>
        /// Facebook the asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task<IActionResult> FacebookAsync()
        {
            return await _webHookHandler.HandleAsync(HttpContext, async (callback, data) =>
            {
                RILogManager.Default.SendJSON("Facebook.RawData", data);
                RILogManager.Default.SendJSON("Facebook.Callback", callback);

                foreach (var entry in callback.Entry)
                {
                    if (entry.Messaging != null)
                    {
                        await HandleMessagingAsync(entry.Messaging);
                    }                    
                }

                return Ok();
            });
        }
              
        #endregion Facebook
    }
}
