﻿using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using QuickBootstrap.Entities;
using QuickBootstrap.Extendsions;
using QuickBootstrap.Filters;
using QuickBootstrap.Helpers;
using QuickBootstrap.Models;
using QuickBootstrap.Services;
using QuickBootstrap.Util;

namespace QuickBootstrap.Controllers
{
    [UserAuthorization]
    public class UserManageController : UserPermissionsController
    {
        #region 私有字段

        private readonly IUserManageService _userManageService = UnityHelper.Instance.Unity.Resolve<IUserManageService>();

        #endregion

        #region action method

        public ActionResult Index()
        {
            return View(_userManageService.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public ActionResult Create(UserCreateRequest model)
        {
            var userPwdHash = model.Password.ToMd5();

            if (!_userManageService.ExistsUser(model.UserName))
            {
                _userManageService.Create(new User
                {
                    UserName = model.UserName,
                    UserPwd = userPwdHash,
                    Nick = model.Nick,
                    CreateTime = DateTime.Now
                });

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("_error", "登录账号已存在");

            return View();
        }

        public ActionResult Delete(string username = "")
        {
            if (username.Length > 0)
            {
                _userManageService.Delete(username);
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}