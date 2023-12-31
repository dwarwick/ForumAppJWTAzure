﻿namespace ForumAppJWTAzure.Client.Services
{
    public interface INotificationService
    {
        event EventHandler<bool> NotificationRead;

        Task<Response<List<T>>> GetAll<T>(string endPoint);

        Task<bool> Put<T>(T value, string endPoint);

        Task<Response<T>> GetSingle<T>(string endPoint);
    }
}