using System;

public class Task

{
    protected int _id;
    protected string _description;
    protected string _status; // todo, in-progres, done
    protected DateTime _createdAt;
    protected DateTime _updatedAt;

    public int Id { 
        get => _id;
        set => _id = value; 
    }
    public string Description {
        get => _description;
        set => _description = value;
    }
    public string Status {
        get => _status; 
        set
        {
            if((value == "todo") || (value == "in-progress") || (value == "done"))
            {
                _status = value;
            }
        }
    }
    public DateTime CreatedAt {
        get => _createdAt;
        set => _createdAt = value;
    }
    public DateTime UpdatedAt {
        get => _updatedAt;
        set => _updatedAt = value;
    }
   

    public Task(int id, string description, string status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Description: {Description}, Status: {Status}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
    }
}
