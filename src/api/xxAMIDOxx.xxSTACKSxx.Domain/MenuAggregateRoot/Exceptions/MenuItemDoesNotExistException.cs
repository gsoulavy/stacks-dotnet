﻿using System;
using Amido.Stacks.Domain;

namespace xxAMIDOxx.xxSTACKSxx.Domain.MenuAggregateRoot.Exceptions
{
    public class MenuItemDoesNotExistException : DomainException
    {
        private MenuItemDoesNotExistException(
            string message
        ) : base(message)
        {
        }

        public override int ExceptionCode => (int)Common.Exceptions.ExceptionCode.MenuItemDoesNotExist;

        public static void Raise(Guid categoryId, Guid menuItemId, string message)
        {
            var exception = new MenuItemDoesNotExistException(
                message ?? $"The item {menuItemId} does not exist in the category '{categoryId}'."
            );

            exception.Data["CategoryId"] = categoryId;
            exception.Data["MenuItemId"] = menuItemId;

            throw exception;
        }

        public static void Raise(Guid categoryId, Guid menuItemId)
        {
            Raise(categoryId, menuItemId, null);
        }
    }
}