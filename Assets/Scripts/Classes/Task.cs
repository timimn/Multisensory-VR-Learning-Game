using System.Collections.Generic;
using System.Linq;

#nullable enable  // Enable nullable types for the class

namespace Game.Task {
    public class Task {
        public string name;
        public bool completed;
        public List<Subtask>? subtasks;

        // Constructor for the Task class
        public Task(string name, List<Subtask>? subtasks = null) {
            this.name = name;
            this.completed = false;
            this.subtasks = subtasks;
        }

        // Method for checking if all subtasks are completed
        public bool AllSubtasksCompleted() {
            return this.subtasks?.All(subtask => subtask.completed) ?? true;
        }

        // Method for checking if subtasks are partly completed
        public bool SubtasksPartlyCompleted() {
            return this.subtasks?.Any(subtask => subtask.partsCompleted > 0) ?? false;
        }
    }

    public class Subtask {
        public string name;
        public bool completed;
        public int parts;
        public int partsCompleted;

        // Constructor for the Subtask class
        public Subtask(string name, int parts = 1) {
            this.name = name;
            this.completed = false;
            this.parts = parts;
            this.partsCompleted = 0;
        }

        // Method for checking if all parts of the subtask are completed
        public bool AllPartsCompleted() {
            return this.partsCompleted == this.parts;
        }
    }
}
