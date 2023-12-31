﻿using System.Runtime.Serialization;

[Serializable]
internal class RestaurantException : Exception
{
    public RestaurantException()
    {
    }

    public RestaurantException(string? message) : base(message)
    {
    }

    public RestaurantException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected RestaurantException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}