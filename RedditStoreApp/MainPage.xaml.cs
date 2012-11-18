﻿using RedditStoreApp.Data;
using RedditStoreApp.Data.Core;
using RedditStoreApp.Data.Factory;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RedditStoreApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
       
            /*
            RedditApi reddit = new RedditApi();
            await reddit.Login(await PasswordVaultWrapper.GetUsername(), await PasswordVaultWrapper.GetPassword());
            Listing<Subreddit> l = await reddit.GetPopularSubredditsListAsync();
            Listing<Subreddit> l1 = await reddit.GetMySubredditsListAsync();
            //await l.More();
           // await l1.Refresh();
            await l[0].Posts.Load();
            await l[0].Posts[1].Comments.Load();
            await l[0].Posts[1].Comments[0].Replies.More();
            await l[0].Posts[1].Comments[0].Replies.Refresh();
            await l[0].Posts[1].Comments.Refresh();
            System.Diagnostics.Debugger.Break();
             * */
        }
    }
}
