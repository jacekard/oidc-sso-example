// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Marvin.IDP
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
             new TestUser
             {
                 SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                 Username = "Wojtek",
                 Password = "pass",

                 Claims = new List<Claim>
                 {
                     new Claim("given_name", "Wojtek"),
                     new Claim("family_name", "Borostowski"),
                     new Claim("address", "Main Road 1"),
                     new Claim("role", "FreeUser")
                 }
             },
             new TestUser
             {
                 SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                 Username = "Jacek",
                 Password = "pass",

                 Claims = new List<Claim>
                 {
                     new Claim("given_name", "Jacek"),
                     new Claim("family_name", "Ardanowski"),
                     new Claim("address", "Big Street 2"),
                     new Claim("role", "PayingUser")
                 }
             },
             new TestUser
             {
                 SubjectId = "ba524d27-bb83-44eb-bdaa-30ced04a1988",
                 Username = "Karol",
                 Password = "pass",
                 Claims = new List<Claim>
                 {
                     new Claim("given_name", "Karol"),
                     new Claim("family_name", "Szczepanski"),
                     new Claim("address", "Big Street 3"),
                     new Claim("role", "PayingUser")
                 }
             }
         };

    }
}