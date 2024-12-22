using System.Data;

class Worker {
    public string name {get; }

    public Worker(string name) => this.name = name;
}

class Task {
    public string name {get; }
    public string description;
    public string status;
    public Worker? asigned;

    public string time;

    public Task(string name, string description) {
        this.name = name;
        this.description = description;
        status = "none";
        time = "none";
    }

    public override string ToString()
    {
        return name + " " + description + " " + status + " " + time;
    }
}

class TaskManager {
    private List<Worker> workers;
    private List<Task> tasks;

    public TaskManager() {
        workers = new List<Worker>();
        tasks = new List<Task>();
    }

    private bool hasTask(string name) {
        return findTask(name) != null;
    }

    private Task? findTask(string name) {
        return tasks.Find(task => task.name == name);
    }

    private Task checkTask(string name) {
        var task = findTask(name);

        if (task == null) {
            throw new Exception("task does not exist");
        }

        return task;
    }

    private bool hasWorker(string name) {
        return findTask(name) != null;
    }

    private Worker? findWorker(string name) {
        return workers.Find(worker => worker.name == name);
    }

    private Worker checkWorker(string name) {
        var worker = findWorker(name);

        if (worker == null) {
            throw new Exception("worker does not exist");
        }

        return worker;
    }

    public void addTask(string name, string description) {
        if (hasTask(name)) {
            throw new Exception("task already exists");
        }

        tasks.Add(new Task(name, description));
    }

    public void addWorker(string name) {
        if (hasWorker(name)) {
            throw new Exception("worker already exists");
        }

        workers.Add(new Worker(name));
    }

    public void assignWorker(string taskName, string workerName) {
        var worker = checkWorker(workerName);
        var task = checkTask(taskName);

        task.asigned = worker;
    }

    public void setStatus(string taskName, string status) {
        var task = checkTask(taskName);

        task.status = status;
    }

    public void setTime(string taskName, string time) {
        var task = checkTask(taskName);

        task.time = time;
    }

    public void listTasksOfWorker(string workerName) {
        var worker = checkWorker(workerName);

        var taskList = tasks.Where(task => task.asigned == worker);

        foreach (var (task, i) in taskList.Select((task, i) => (task, i))) {
            Console.WriteLine(i + ": " + task.ToString());
        }
    }

    public void listAllTasks() {
        foreach (var worker in workers) {
            Console.WriteLine(worker.name + ":");
            listTasksOfWorker(worker.name);
        }
    }
}

class Program {
    static void Main(string[] args) 
    {
        TaskManager manager = new TaskManager();

        string command = "";
        string[] query = [];

        while (command != "q" && command != "quit") {
            try {
                if (command == "add") {
                    if (query[1] == "task") {
                        manager.addTask(query[2], query[3]);
                    } else if (query[1] == "worker") {
                        manager.addWorker(query[2]);
                    }
                }

                else if (command == "set") {
                    if (query[1] == "status") {
                        manager.setStatus(query[2], query[3]);
                    } else if (query[1] == "time") {
                        manager.setTime(query[2], query[3]);
                    }
                }

                else if (command == "assign") {
                    manager.assignWorker(query[1], query[2]);
                }

                else if (command == "list") {
                    if (query.Length == 1) {
                        manager.listAllTasks();
                    } else {
                        manager.listTasksOfWorker(query[1]);
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            string? s = Console.ReadLine();

            if (string.IsNullOrEmpty(s)) {
                query = [];
            } else {
                query = s.Split(" ");
            }

            if (query.Length > 0) {
                command = query[0];
            } else {
                command = "";
            }
        }
    }
}
