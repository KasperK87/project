using RogueSharp;

namespace ResidentSurvivor{

    //IComponent_Entity is a component that can be added to any entity
    //it will make the entity hostile to the player
    //moving towards the player and attacking when in range
    class IComponent_Hostile : SadConsole.Components.UpdateComponent {
        GameObject parent;
        private RogueSharp.Path? _cells;
        public static RogueSharp.GoalMap _goalMap;
        private Point mapSize;
        private Floor _dungeon;

        private  SadRogue.Primitives.Point previousPosition;

        private Point[] _goals; 
        //private bool followingPath;
        private UInt64 turn;
        private IComponent_Entity entity;



        public IComponent_Hostile(SadConsole.Entities.Entity setParent, IComponent_Entity setEntity){
            //followingPath = false;

            this.parent = (GameObject)setParent;
            this.entity = setEntity;

            previousPosition = parent.Position;
        }

        public IComponent_Hostile(SadConsole.Entities.Entity setParent, IComponent_Entity setEntity, Floor dungeon)
            : this(setParent, setEntity){
            _goalMap = new RogueSharp.GoalMap(dungeon.GetDungeonMap(), true);

            mapSize = new Point(dungeon.GetDungeonMap().Width, dungeon.GetDungeonMap().Height);

            _dungeon = dungeon;

            _goals = new Point[1];
            
            //set some random goals
            int goals = 1;
            while (goals > 0){
                int x = parent.rand.Next(2,dungeon.GetDungeonMap().Width-1);
                int y = parent.rand.Next(2,dungeon.GetDungeonMap().Height-1);
                if (dungeon.GetDungeonMap().IsWalkable(x,y)){
                    _goals[goals-1] = new Point(x,y);
                    goals--;
                }
            }
        }

        
        public override void Update(SadConsole.IScreenObject p, TimeSpan delta){
            //every turn we do the following once
            if (this.turn < Game.UIManager.currentFloor.turn && entity.currHP > 0){
                if (parent.GetSadComponent<IComponent_Entity>() != null){
                    if (parent.GetSadComponent<IComponent_Entity>().state == entityState.wandering && _goalMap != null){
                        followPath();
                    }
                    if (parent.GetSadComponent<IComponent_Entity>().state == entityState.hostile){
                        followPath();                  
                    } else if (Game.UIManager.currentFloor.GetDungeonMap().IsInFov(parent.Position.X, parent.Position.Y)){
                        
                        //if (_cells != null)
                            //need to check distance to player in another way then _cells.length
                            if (7 >= Game.UIManager.currentFloor.getDistanceToPlayer(parent.Position.X, parent.Position.Y) &&
                                parent.rand.Next(1,20) > 15){
                                parent.GetSadComponent<IComponent_Entity>().state = entityState.hostile;
                                System.Console.WriteLine("HOSTILE!!!");
                            }
                    }
                }

                if (parent.GetSadComponent<IComponent_Entity>().state == entityState.wandering &&
                _goalMap != null && _goals != null && previousPosition == parent.Position ){
                //set _cells to goalmap
                //System.Console.WriteLine("GoalMap");

                for (int i = 0; i < _goals.Length; i++){
                    //if (_goals[i].X == parent.Position.X && _goals[i].Y == parent.Position.Y){
                        bool newGoal = false;
                        
                        while (!newGoal){
                            int x = parent.rand.Next(2,mapSize.X-1);
                            int y = parent.rand.Next(2,mapSize.Y-1);
                            if (_dungeon.GetDungeonMap().IsWalkable(x,y)){
                                _goals[i] = new Point(x,y);
                                newGoal = true;
                            }
                        }
                    //}
                }

                if (_cells == null || _cells.Length >= 1)
                try {
                    Game.UIManager.currentFloor.updateHostilesGoalmap();
                    _goalMap.ClearGoals();
                    foreach (Point goal in _goals){
                        _goalMap.AddGoal(goal.X, goal.Y, 1);
                    }

                    _goalMap.RemoveObstacle(parent.Position.X, parent.Position.Y);

                    _cells = _goalMap.TryFindPath(parent.Position.X, parent.Position.Y);
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                } catch (RogueSharp.PathNotFoundException) {
                    _cells = null;
                }
            }
                previousPosition = parent.Position;;
                this.turn = Game.UIManager.currentFloor.turn;
            }

            if (!Game.UIManager.currentFloor.GetDungeonMap().IsInFov(parent.Position.X, parent.Position.Y)){
                parent.Appearance.Foreground = SadRogue.Primitives.Color.Transparent;
            } else if (parent.GetSadComponent<IComponent_Entity>().state == entityState.hostile){
                //parent.Appearance.Foreground = SadRogue.Primitives.Color.White;
                _cells = Game.UIManager.currentFloor.pathToPlayerFrom(parent.Position.X, parent.Position.Y);
            }
        }

        private void followPath(){
            if ( _cells != null && _cells.Length >= 1)
            {   
                if (!checkIfNextStepIsValid()){
                    _cells = null;
                    return;
                }
                try {
                    _cells.StepForward();
                    //check is there is a monster
                    var monster = (GameObject)Game.UIManager.currentFloor.GetMonsterAt(_cells.CurrentStep.X, _cells.CurrentStep.Y);
                    if (monster == null){
                        parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);     
                    } else {
                        if (monster.Walkable){
                            parent.Position = new SadRogue.Primitives.Point(_cells.CurrentStep.X, _cells.CurrentStep.Y);;
                        } else if (monster.isPlayer){
                            //System.Console.WriteLine("Monster Attack!!!");
                            Attack(monster);
                        } else {
                            monster.Interact();
                        }
                        
                    }             
                } catch (RogueSharp.NoMoreStepsException) {
                    _cells = null;
                    //followingPath = false;
                }
            }
        }

        private bool checkIfNextStepIsValid(){
            if (parent.Position.X == _cells.End.X && parent.Position.Y == _cells.End.Y){
                return false;
            }
            return true;
        }

        public void Attack(GameObject target){
            if (target.GetSadComponent<IComponent_Entity>() != null){
                parent.Attack(target);
            } else {
                target.Interact();
            }
            System.Console.WriteLine("Monster Attack!!!");
        }
    }
}