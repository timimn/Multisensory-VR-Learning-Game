using System.Collections.Generic;

namespace Game.Task {
    public class Task {
        public string name;
        public bool completed;
        public int subtasksCompleted;
        public int subtasksTarget;

        public Task(string name, int target = 1) {
            this.name = name;
            this.completed = false;
            this.subtasksCompleted = 0;
            this.subtasksTarget = target;
        }

        public bool AllSubtasksCompleted() {
            return this.subtasksCompleted == this.subtasksTarget;
        }
    }
}
