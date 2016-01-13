﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotNews.Providers
{
    public interface IAuthProvider
    {
        bool IsLoggedIn { get; }
        bool Login(string username, string password);
        void Logout();
    }
}
