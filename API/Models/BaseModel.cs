using System;

namespace API.Models;

public class BaseModel
{
    public class DoesNotExists : Exception {    
        public DoesNotExists(string message) : base(message) {}
    }
}
