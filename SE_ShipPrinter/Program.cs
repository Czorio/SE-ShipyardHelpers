using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // Change this if you want the script to search for a different tag
        private string searchTag = "{SP}";

        // Blocks
        private IMyProjector projector;
        private IMyAssembler mainAssembler;

        // Script helpers
        private Dictionary<MyDefinitionId, float> neededComponents;
        private Dictionary<string, Dictionary<string, float>> componentsForBlock;

        public Program()
        {
            // Blocks
            List<IMyTerminalBlock> myTerminalBlocks = new List<IMyTerminalBlock>();
            GridTerminalSystem.SearchBlocksOfName(searchTag, myTerminalBlocks, x => x is IMyProjector);
            if (myTerminalBlocks.Count == 0)
            {
                Echo($"Cannot find suitable projector. Have you used \"{searchTag}\" in the name?");
                return;
            }
            else
            {
                projector = myTerminalBlocks[0] as IMyProjector;
            }

            GridTerminalSystem.SearchBlocksOfName(searchTag, myTerminalBlocks, x => x is IMyAssembler);
            if (myTerminalBlocks.Count == 0)
            {
                Echo($"Cannot find suitable assembler. Have you used \"{searchTag}\" in the name?");
                return;
            }
            else
            {
                mainAssembler = myTerminalBlocks[0] as IMyAssembler;
            }

            // Script helpers
            // Not teriffically pretty
            neededComponents = new Dictionary<MyDefinitionId, float>()
            {
                { CreateComponentBlueprint("SteelPlate").Value, 0.0f },
                { CreateComponentBlueprint("PowerCell").Value, 0.0f },
                { CreateComponentBlueprint("LargeTube").Value, 0.0f },
                { CreateComponentBlueprint("SmallTube").Value, 0.0f },
                { CreateComponentBlueprint("Display").Value, 0.0f },
                { CreateComponentBlueprint("MetalGrid").Value, 0.0f },
                { CreateComponentBlueprint("InteriorPlate").Value, 0.0f },
                { CreateComponentBlueprint("BulletproofGlass").Value, 0.0f },
                { CreateComponentBlueprint("Superconductor").Value, 0.0f },
                { CreateComponentBlueprint("SolarCell").Value, 0.0f },
                { CreateComponentBlueprint("RadioCommunication").Value, 0.0f },
                { CreateComponentBlueprint("Computer").Value, 0.0f },
                { CreateComponentBlueprint("Reactor").Value, 0.0f },
                { CreateComponentBlueprint("Detector").Value, 0.0f },
                { CreateComponentBlueprint("Construction").Value, 0.0f },
                { CreateComponentBlueprint("Thrust").Value, 0.0f },
                { CreateComponentBlueprint("Motor").Value, 0.0f },
                { CreateComponentBlueprint("Explosives").Value, 0.0f },
                { CreateComponentBlueprint("Girder").Value, 0.0f },
                { CreateComponentBlueprint("GravityGenerator").Value, 0.0f },
                { CreateComponentBlueprint("Medical").Value, 0.0f },
                { CreateComponentBlueprint("NATO_25x184mm").Value, 0.0f },
                { CreateComponentBlueprint("NATO_5p56x45mm").Value, 0.0f }
            };

            componentsForBlock = new Dictionary<string, Dictionary<string, float>>()
            {
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 13.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 21.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeRoundArmor_Slope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 13.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeRoundArmor_Corner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeRoundArmor_CornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 21.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "MetalGrid", 50.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 75.0f },
                        { "MetalGrid", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "MetalGrid", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 125.0f },
                        { "MetalGrid", 50.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "MetalGrid", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHalfArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyHalfArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 75.0f },
                        { "MetalGrid", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHalfSlopeArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyHalfSlopeArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 45.0f },
                        { "MetalGrid", 15.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/HalfArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/HeavyHalfArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/HalfSlopeArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/HeavyHalfSlopeArmorBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 13.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 21.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 130.0f },
                        { "MetalGrid", 50.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 125.0f },
                        { "MetalGrid", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 140.0f },
                        { "MetalGrid", 50.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundSlope",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCornerInv",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 19.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 22.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 112.0f },
                        { "MetalGrid", 45.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 35.0f },
                        { "MetalGrid", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 55.0f },
                        { "MetalGrid", 15.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 19.0f },
                        { "MetalGrid", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 133.0f },
                        { "MetalGrid", 45.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 94.0f },
                        { "MetalGrid", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Base",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Tip",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "MetalGrid", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MyProgrammableBlock/SmallProgrammableBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 2.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 1.0f },
                        { "Display", 1.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MyObjectBuilder_Projector/LargeProjector",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 21.0f },
                        { "Construction", 4.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 1.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MyObjectBuilder_Projector/SmallProjector",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 2.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 1.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_SensorBlock/SmallBlockSensor",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 5.0f },
                        { "Construction", 5.0f },
                        { "Computer", 6.0f },
                        { "RadioCommunication", 4.0f },
                        { "Detector", 6.0f },
                        { "SteelPlate", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_SensorBlock/LargeBlockSensor",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 5.0f },
                        { "Construction", 5.0f },
                        { "Computer", 6.0f },
                        { "RadioCommunication", 4.0f },
                        { "Detector", 6.0f },
                        { "SteelPlate", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_SoundBlock/SmallBlockSoundBlock",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 4.0f },
                        { "Construction", 6.0f },
                        { "Computer", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_SoundBlock/LargeBlockSoundBlock",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 4.0f },
                        { "Construction", 6.0f },
                        { "Computer", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_ButtonPanel/ButtonPanelLarge",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 20.0f },
                        { "Computer", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_ButtonPanel/ButtonPanelSmall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 2.0f },
                        { "Construction", 2.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TimerBlock/TimerBlockLarge",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 6.0f },
                        { "Construction", 30.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_TimerBlock/TimerBlockSmall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 2.0f },
                        { "Construction", 3.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MyProgrammableBlock/LargeProgrammableBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 21.0f },
                        { "Construction", 4.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 1.0f },
                        { "Display", 1.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "LargeTube", 40.0f },
                        { "SmallTube", 60.0f },
                        { "Construction", 30.0f },
                        { "Computer", 8.0f },
                        { "RadioCommunication", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_Beacon/LargeBlockBeacon",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 20.0f },
                        { "Computer", 10.0f },
                        { "RadioCommunication", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_Beacon/SmallBlockBeacon",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 1.0f },
                        { "SmallTube", 1.0f },
                        { "Computer", 1.0f },
                        { "RadioCommunication", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_RadioAntenna/SmallBlockRadioAntenna",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                        { "SmallTube", 1.0f },
                        { "Construction", 2.0f },
                        { "Computer", 1.0f },
                        { "RadioCommunication", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_RemoteControl/LargeBlockRemoteControl",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 10.0f },
                        { "Motor", 1.0f },
                        { "Computer", 15.0f },
                    }
                },
                {
                    "MyObjectBuilder_RemoteControl/SmallBlockRemoteControl",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 2.0f },
                        { "Construction", 1.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_LaserAntenna/LargeBlockLaserAntenna",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 50.0f },
                        { "Construction", 40.0f },
                        { "Motor", 16.0f },
                        { "Detector", 30.0f },
                        { "RadioCommunication", 20.0f },
                        { "Superconductor", 100.0f },
                        { "Computer", 50.0f },
                        { "BulletproofGlass", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_LaserAntenna/SmallBlockLaserAntenna",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "SmallTube", 10.0f },
                        { "Construction", 10.0f },
                        { "Motor", 5.0f },
                        { "RadioCommunication", 5.0f },
                        { "Superconductor", 10.0f },
                        { "Computer", 30.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_TerminalBlock/ControlPanel",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                        { "Construction", 1.0f },
                        { "Computer", 1.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TerminalBlock/SmallControlPanel",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                        { "Construction", 1.0f },
                        { "Computer", 1.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockCockpit",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                        { "Motor", 2.0f },
                        { "Computer", 100.0f },
                        { "Display", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockCockpitSeat",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 20.0f },
                        { "Motor", 1.0f },
                        { "Display", 8.0f },
                        { "Computer", 100.0f },
                        { "BulletproofGlass", 60.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/SmallBlockCockpit",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "Construction", 10.0f },
                        { "Motor", 1.0f },
                        { "Display", 5.0f },
                        { "Computer", 15.0f },
                        { "BulletproofGlass", 30.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/DBSmallBlockFighterCockpit",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 20.0f },
                        { "Motor", 1.0f },
                        { "SteelPlate", 20.0f },
                        { "MetalGrid", 10.0f },
                        { "InteriorPlate", 15.0f },
                        { "Display", 4.0f },
                        { "Computer", 20.0f },
                        { "BulletproofGlass", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/CockpitOpen",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                        { "Motor", 2.0f },
                        { "Computer", 100.0f },
                        { "Display", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_Gyro/LargeBlockGyro",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 600.0f },
                        { "Construction", 40.0f },
                        { "LargeTube", 4.0f },
                        { "MetalGrid", 50.0f },
                        { "Motor", 4.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_Gyro/SmallBlockGyro",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 5.0f },
                        { "LargeTube", 1.0f },
                        { "Motor", 2.0f },
                        { "Computer", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/OpenCockpitSmall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                        { "Motor", 1.0f },
                        { "Computer", 15.0f },
                        { "Display", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/OpenCockpitLarge",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                        { "Motor", 2.0f },
                        { "Computer", 100.0f },
                        { "Display", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockDesk",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockDeskCorner",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockDeskChairless",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockDeskChairlessCorner",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_Kitchen/LargeBlockKitchen",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 6.0f },
                        { "Motor", 6.0f },
                        { "BulletproofGlass", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CryoChamber/LargeBlockBed",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                        { "SmallTube", 8.0f },
                        { "BulletproofGlass", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/LargeBlockLockerRoom",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                        { "Display", 4.0f },
                        { "BulletproofGlass", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/LargeBlockLockerRoomCorner",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 25.0f },
                        { "Construction", 30.0f },
                        { "Display", 4.0f },
                        { "BulletproofGlass", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Planter/LargeBlockPlanters",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 8.0f },
                        { "BulletproofGlass", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockCouch",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockCouchCorner",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 35.0f },
                        { "Construction", 35.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/LargeBlockLockers",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                        { "Display", 3.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockBathroomOpen",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 30.0f },
                        { "SmallTube", 8.0f },
                        { "Motor", 4.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockBathroom",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 8.0f },
                        { "Motor", 4.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockToilet",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 15.0f },
                        { "SmallTube", 2.0f },
                        { "Motor", 2.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Projector/LargeBlockConsole",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "Computer", 8.0f },
                        { "Display", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/SmallBlockCockpitIndustrial",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "Construction", 20.0f },
                        { "MetalGrid", 10.0f },
                        { "Motor", 2.0f },
                        { "Display", 6.0f },
                        { "Computer", 20.0f },
                        { "BulletproofGlass", 60.0f },
                        { "SmallTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/LargeBlockCockpitIndustrial",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "MetalGrid", 15.0f },
                        { "Motor", 2.0f },
                        { "Display", 10.0f },
                        { "Computer", 60.0f },
                        { "BulletproofGlass", 80.0f },
                        { "SmallTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_VendingMachine/FoodDispenser",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 10.0f },
                        { "Motor", 4.0f },
                        { "Display", 10.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Jukebox/Jukebox",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 15.0f },
                        { "Construction", 10.0f },
                        { "Computer", 4.0f },
                        { "Display", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_LCDPanelsBlock/LabEquipment",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 15.0f },
                        { "Construction", 15.0f },
                        { "Motor", 1.0f },
                        { "BulletproofGlass", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Shower",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 12.0f },
                        { "BulletproofGlass", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/WindowWall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 10.0f },
                        { "BulletproofGlass", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/WindowWallLeft",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "Construction", 10.0f },
                        { "BulletproofGlass", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/WindowWallRight",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "Construction", 10.0f },
                        { "BulletproofGlass", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_LCDPanelsBlock/MedicalStation",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 15.0f },
                        { "Construction", 15.0f },
                        { "Motor", 2.0f },
                        { "Medical", 1.0f },
                        { "Display", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/TransparentLCDLarge",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 8.0f },
                        { "Computer", 6.0f },
                        { "Display", 10.0f },
                        { "BulletproofGlass", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/TransparentLCDSmall",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 4.0f },
                        { "Computer", 4.0f },
                        { "Display", 3.0f },
                        { "BulletproofGlass", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Catwalk",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 16.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/CatwalkCorner",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 24.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 32.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/CatwalkStraight",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 24.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 32.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/CatwalkWall",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 20.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 26.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/CatwalkRailingEnd",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 28.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 38.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/CatwalkRailingHalfRight",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 28.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 36.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/CatwalkRailingHalfLeft",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 28.0f },
                        { "Girder", 4.0f },
                        { "SmallTube", 36.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/GratedStairs",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 22.0f },
                        { "SmallTube", 12.0f },
                        { "InteriorPlate", 16.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/GratedHalfStairs",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 20.0f },
                        { "SmallTube", 6.0f },
                        { "InteriorPlate", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/GratedHalfStairsMirrored",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 20.0f },
                        { "SmallTube", 6.0f },
                        { "InteriorPlate", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/RailingStraight",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 8.0f },
                        { "SmallTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/RailingDouble",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 16.0f },
                        { "SmallTube", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/RailingCorner",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 16.0f },
                        { "SmallTube", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/RailingDiagonal",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 12.0f },
                        { "SmallTube", 9.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/RailingHalfRight",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 8.0f },
                        { "SmallTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/RailingHalfLeft",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 8.0f },
                        { "SmallTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_ReflectorLight/RotatingLightLarge",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_ReflectorLight/RotatingLightSmall",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Freight1",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 6.0f },
                        { "Construction", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Freight2",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 12.0f },
                        { "Construction", 16.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Freight3",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 18.0f },
                        { "Construction", 24.0f },
                    }
                },
                {
                    "MyObjectBuilder_Door/(null)",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 2.0f },
                        { "Display", 1.0f },
                        { "Computer", 2.0f },
                        { "SteelPlate", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_AirtightHangarDoor/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 350.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 40.0f },
                        { "Motor", 16.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_AirtightSlideDoor/LargeBlockSlideDoor",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 4.0f },
                        { "Display", 1.0f },
                        { "Computer", 2.0f },
                        { "BulletproofGlass", 15.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/ArmorCenter",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 140.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/ArmorCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 120.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/ArmorInvCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 135.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/ArmorSide",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 130.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallArmorCenter",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallArmorCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallArmorInvCorner",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallArmorSide",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_StoreBlock/StoreBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 20.0f },
                        { "Motor", 6.0f },
                        { "Display", 4.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_SafeZoneBlock/SafeZoneBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 800.0f },
                        { "Construction", 180.0f },
                        { "GravityGenerator", 10.0f },
                        { "ZoneChip", 5.0f },
                        { "MetalGrid", 80.0f },
                        { "Computer", 120.0f },
                    }
                },
                {
                    "MyObjectBuilder_ContractBlock/ContractBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 20.0f },
                        { "Motor", 6.0f },
                        { "Display", 4.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_VendingMachine/VendingMachine",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 10.0f },
                        { "Motor", 4.0f },
                        { "Display", 4.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_StoreBlock/AtmBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 20.0f },
                        { "Motor", 2.0f },
                        { "Computer", 10.0f },
                        { "Display", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "Construction", 30.0f },
                        { "PowerCell", 80.0f },
                        { "Computer", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_BatteryBlock/SmallBlockBatteryBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 5.0f },
                        { "PowerCell", 20.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_BatteryBlock/SmallBlockSmallBatteryBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 2.0f },
                        { "PowerCell", 2.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Reactor/SmallBlockSmallGenerator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "Construction", 10.0f },
                        { "MetalGrid", 2.0f },
                        { "LargeTube", 1.0f },
                        { "Reactor", 3.0f },
                        { "Motor", 1.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Reactor/SmallBlockLargeGenerator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 60.0f },
                        { "Construction", 9.0f },
                        { "MetalGrid", 9.0f },
                        { "LargeTube", 3.0f },
                        { "Reactor", 95.0f },
                        { "Motor", 5.0f },
                        { "Computer", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_Reactor/LargeBlockSmallGenerator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "Construction", 40.0f },
                        { "MetalGrid", 4.0f },
                        { "LargeTube", 8.0f },
                        { "Reactor", 100.0f },
                        { "Motor", 6.0f },
                        { "Computer", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_Reactor/LargeBlockLargeGenerator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1000.0f },
                        { "Construction", 70.0f },
                        { "MetalGrid", 40.0f },
                        { "LargeTube", 40.0f },
                        { "Superconductor", 100.0f },
                        { "Reactor", 2000.0f },
                        { "Motor", 20.0f },
                        { "Computer", 75.0f },
                    }
                },
                {
                    "MyObjectBuilder_HydrogenEngine/LargeHydrogenEngine",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 100.0f },
                        { "Construction", 70.0f },
                        { "LargeTube", 12.0f },
                        { "SmallTube", 20.0f },
                        { "Motor", 12.0f },
                        { "Computer", 4.0f },
                        { "PowerCell", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_HydrogenEngine/SmallHydrogenEngine",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 20.0f },
                        { "LargeTube", 4.0f },
                        { "SmallTube", 6.0f },
                        { "Motor", 4.0f },
                        { "Computer", 1.0f },
                        { "PowerCell", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_WindTurbine/LargeBlockWindTurbine",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 40.0f },
                        { "Motor", 8.0f },
                        { "Construction", 20.0f },
                        { "Girder", 24.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_SolarPanel/LargeBlockSolarPanel",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 14.0f },
                        { "Girder", 12.0f },
                        { "Computer", 4.0f },
                        { "SolarCell", 32.0f },
                        { "BulletproofGlass", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_SolarPanel/SmallBlockSolarPanel",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 2.0f },
                        { "Girder", 4.0f },
                        { "Computer", 1.0f },
                        { "SolarCell", 8.0f },
                        { "BulletproofGlass", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_GravityGenerator/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "GravityGenerator", 6.0f },
                        { "Construction", 60.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 6.0f },
                        { "Computer", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_GravityGeneratorSphere/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "GravityGenerator", 6.0f },
                        { "Construction", 60.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 6.0f },
                        { "Computer", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_VirtualMass/VirtualMassLarge",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 90.0f },
                        { "Superconductor", 20.0f },
                        { "Construction", 30.0f },
                        { "Computer", 20.0f },
                        { "GravityGenerator", 9.0f },
                    }
                },
                {
                    "MyObjectBuilder_VirtualMass/VirtualMassSmall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "Superconductor", 2.0f },
                        { "Construction", 2.0f },
                        { "Computer", 2.0f },
                        { "GravityGenerator", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_SpaceBall/SpaceBallLarge",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 225.0f },
                        { "Construction", 30.0f },
                        { "Computer", 20.0f },
                        { "GravityGenerator", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_SpaceBall/SpaceBallSmall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 70.0f },
                        { "Construction", 10.0f },
                        { "Computer", 7.0f },
                        { "GravityGenerator", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Passage/(null)",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 74.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 48.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeStairs",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 50.0f },
                        { "Construction", 30.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeRamp",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 70.0f },
                        { "Construction", 16.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeSteelCatwalk",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 27.0f },
                        { "Construction", 5.0f },
                        { "SmallTube", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeSteelCatwalk2Sides",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 32.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeSteelCatwalkCorner",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 32.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeSteelCatwalkPlate",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 23.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 17.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeCoverWall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeCoverWallHalf",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeBlockInteriorWall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 25.0f },
                        { "Construction", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeInteriorPillar",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 25.0f },
                        { "Construction", 10.0f },
                        { "SmallTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/PassengerSeatLarge",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_Cockpit/PassengerSeatSmall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_Ladder2/(null)",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Ladder2/LadderSmall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallTextPanel",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Construction", 4.0f },
                        { "Computer", 4.0f },
                        { "Display", 3.0f },
                        { "BulletproofGlass", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallLCDPanelWide",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Construction", 8.0f },
                        { "Computer", 8.0f },
                        { "Display", 6.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallLCDPanel",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Construction", 4.0f },
                        { "Computer", 4.0f },
                        { "Display", 3.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_1",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 5.0f },
                        { "Computer", 3.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_2",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 5.0f },
                        { "Computer", 3.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_1",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 5.0f },
                        { "Computer", 3.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_2",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 5.0f },
                        { "Computer", 3.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_1",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                        { "Computer", 2.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_2",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                        { "Computer", 2.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_1",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                        { "Computer", 2.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_2",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                        { "Computer", 2.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeTextPanel",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Construction", 6.0f },
                        { "Computer", 6.0f },
                        { "Display", 10.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeLCDPanel",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Construction", 6.0f },
                        { "Computer", 6.0f },
                        { "Display", 10.0f },
                        { "BulletproofGlass", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_TextPanel/LargeLCDPanelWide",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 2.0f },
                        { "Construction", 12.0f },
                        { "Computer", 12.0f },
                        { "Display", 20.0f },
                        { "BulletproofGlass", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_ReflectorLight/LargeBlockFrontLight",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "LargeTube", 2.0f },
                        { "InteriorPlate", 20.0f },
                        { "Construction", 15.0f },
                        { "BulletproofGlass", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_ReflectorLight/SmallBlockFrontLight",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1.0f },
                        { "LargeTube", 1.0f },
                        { "InteriorPlate", 1.0f },
                        { "Construction", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorLight/SmallLight",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorLight/SmallBlockSmallLight",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorLight/LargeBlockLight_1corner",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorLight/LargeBlockLight_2corner",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorLight/SmallBlockLight_1corner",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorLight/SmallBlockLight_2corner",
                    new Dictionary<string, float>()
                    {
                        { "Construction", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenTank/OxygenTankSmall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                        { "LargeTube", 8.0f },
                        { "SmallTube", 10.0f },
                        { "Computer", 8.0f },
                        { "Construction", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenTank/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "LargeTube", 40.0f },
                        { "SmallTube", 60.0f },
                        { "Computer", 8.0f },
                        { "Construction", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenTank/LargeHydrogenTank",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 280.0f },
                        { "LargeTube", 80.0f },
                        { "SmallTube", 60.0f },
                        { "Computer", 8.0f },
                        { "Construction", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenTank/SmallHydrogenTank",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "LargeTube", 40.0f },
                        { "SmallTube", 60.0f },
                        { "Computer", 8.0f },
                        { "Construction", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_AirVent/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 45.0f },
                        { "Construction", 20.0f },
                        { "Motor", 10.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_AirVent/SmallAirVent",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 10.0f },
                        { "Motor", 2.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/SmallBlockSmallContainer",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 3.0f },
                        { "Construction", 1.0f },
                        { "Computer", 1.0f },
                        { "Motor", 1.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/SmallBlockMediumContainer",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 30.0f },
                        { "Construction", 10.0f },
                        { "Computer", 4.0f },
                        { "Motor", 4.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/SmallBlockLargeContainer",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 75.0f },
                        { "Construction", 25.0f },
                        { "Computer", 6.0f },
                        { "Motor", 8.0f },
                        { "Display", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/LargeBlockSmallContainer",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 40.0f },
                        { "Construction", 40.0f },
                        { "MetalGrid", 4.0f },
                        { "SmallTube", 20.0f },
                        { "Motor", 4.0f },
                        { "Display", 1.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CargoContainer/LargeBlockLargeContainer",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 360.0f },
                        { "Construction", 80.0f },
                        { "MetalGrid", 24.0f },
                        { "SmallTube", 60.0f },
                        { "Motor", 20.0f },
                        { "Display", 1.0f },
                        { "Computer", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_Conveyor/SmallBlockConveyor",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 4.0f },
                        { "Construction", 4.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Conveyor/LargeBlockConveyor",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "SmallTube", 20.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Collector/Collector",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 45.0f },
                        { "Construction", 50.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 8.0f },
                        { "Display", 4.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Collector/CollectorSmall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 35.0f },
                        { "Construction", 35.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 8.0f },
                        { "Display", 2.0f },
                        { "Computer", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipConnector/Connector",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 8.0f },
                        { "Computer", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipConnector/ConnectorSmall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                        { "Construction", 4.0f },
                        { "SmallTube", 2.0f },
                        { "Motor", 1.0f },
                        { "Computer", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipConnector/ConnectorMedium",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 21.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 6.0f },
                        { "Motor", 6.0f },
                        { "Computer", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorTube",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 14.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmall",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Motor", 1.0f },
                        { "Construction", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorTubeMedium",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 10.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 10.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorFrameMedium",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 5.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 5.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorTubeCurved",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 14.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorTubeSmallCurved",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 1.0f },
                        { "Motor", 1.0f },
                        { "Construction", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorConnector/ConveyorTubeCurvedMedium",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 7.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 10.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Conveyor/SmallShipConveyorHub",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 25.0f },
                        { "Construction", 45.0f },
                        { "SmallTube", 25.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorSorter/LargeBlockConveyorSorter",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 50.0f },
                        { "Construction", 120.0f },
                        { "SmallTube", 50.0f },
                        { "Computer", 20.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorSorter/MediumBlockConveyorSorter",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 5.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 5.0f },
                        { "Computer", 5.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ConveyorSorter/SmallBlockConveyorSorter",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 5.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 5.0f },
                        { "Computer", 5.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_PistonBase/LargePistonBase",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 15.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ExtendedPistonBase/LargePistonBase",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 15.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_PistonTop/LargePistonTop",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "LargeTube", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_PistonBase/SmallPistonBase",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 4.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 2.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_ExtendedPistonBase/SmallPistonBase",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 4.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 2.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_PistonTop/SmallPistonTop",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorStator/LargeStator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 15.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorRotor/LargeRotor",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "LargeTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorStator/SmallStator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "Construction", 5.0f },
                        { "SmallTube", 1.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorRotor/SmallRotor",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "SmallTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorAdvancedStator/LargeAdvancedStator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 15.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorAdvancedRotor/LargeAdvancedRotor",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "LargeTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorAdvancedStator/SmallAdvancedStator",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "Construction", 5.0f },
                        { "SmallTube", 1.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorAdvancedRotor/SmallAdvancedRotor",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "LargeTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_MedicalRoom/LargeMedicalRoom",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 240.0f },
                        { "Construction", 80.0f },
                        { "MetalGrid", 60.0f },
                        { "SmallTube", 20.0f },
                        { "LargeTube", 5.0f },
                        { "Display", 10.0f },
                        { "Computer", 10.0f },
                        { "Medical", 15.0f },
                    }
                },
                {
                    "MyObjectBuilder_CryoChamber/LargeBlockCryoChamber",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 40.0f },
                        { "Construction", 20.0f },
                        { "Motor", 8.0f },
                        { "Display", 8.0f },
                        { "Medical", 3.0f },
                        { "Computer", 30.0f },
                        { "BulletproofGlass", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_CryoChamber/SmallBlockCryoChamber",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 20.0f },
                        { "Construction", 10.0f },
                        { "Motor", 4.0f },
                        { "Display", 4.0f },
                        { "Medical", 3.0f },
                        { "Computer", 15.0f },
                        { "BulletproofGlass", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_Refinery/LargeRefinery",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 1200.0f },
                        { "Construction", 40.0f },
                        { "LargeTube", 20.0f },
                        { "Motor", 16.0f },
                        { "MetalGrid", 20.0f },
                        { "Computer", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_Refinery/Blast Furnace",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 120.0f },
                        { "Construction", 20.0f },
                        { "Motor", 10.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenGenerator/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 120.0f },
                        { "Construction", 5.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 4.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenGenerator/OxygenGeneratorSmall",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 8.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 1.0f },
                        { "Computer", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_Assembler/LargeAssembler",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 140.0f },
                        { "Construction", 80.0f },
                        { "Motor", 20.0f },
                        { "Display", 10.0f },
                        { "MetalGrid", 10.0f },
                        { "Computer", 160.0f },
                    }
                },
                {
                    "MyObjectBuilder_Assembler/BasicAssembler",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 80.0f },
                        { "Construction", 40.0f },
                        { "Motor", 10.0f },
                        { "Display", 4.0f },
                        { "Computer", 80.0f },
                    }
                },
                {
                    "MyObjectBuilder_SurvivalKit/SurvivalKitLarge",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 2.0f },
                        { "Medical", 3.0f },
                        { "Motor", 4.0f },
                        { "Display", 1.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_SurvivalKit/SurvivalKit",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 6.0f },
                        { "Construction", 2.0f },
                        { "Medical", 3.0f },
                        { "Motor", 4.0f },
                        { "Display", 1.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 40.0f },
                        { "BulletproofGlass", 100.0f },
                        { "LargeTube", 20.0f },
                        { "SmallTube", 10.0f },
                        { "Construction", 20.0f },
                        { "Computer", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_UpgradeModule/LargeProductivityModule",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 100.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 20.0f },
                        { "Computer", 60.0f },
                        { "Motor", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_UpgradeModule/LargeEffectivenessModule",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 100.0f },
                        { "Construction", 50.0f },
                        { "SmallTube", 15.0f },
                        { "Superconductor", 20.0f },
                        { "Motor", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_UpgradeModule/LargeEnergyModule",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 100.0f },
                        { "Construction", 40.0f },
                        { "SmallTube", 20.0f },
                        { "PowerCell", 20.0f },
                        { "Motor", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/SmallBlockSmallThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 2.0f },
                        { "LargeTube", 1.0f },
                        { "Thrust", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/SmallBlockLargeThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "Construction", 2.0f },
                        { "LargeTube", 5.0f },
                        { "Thrust", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/LargeBlockSmallThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 60.0f },
                        { "LargeTube", 8.0f },
                        { "Thrust", 80.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/LargeBlockLargeThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "Construction", 100.0f },
                        { "LargeTube", 40.0f },
                        { "Thrust", 960.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "Construction", 180.0f },
                        { "MetalGrid", 250.0f },
                        { "LargeTube", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/LargeBlockSmallHydrogenThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 60.0f },
                        { "MetalGrid", 40.0f },
                        { "LargeTube", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/SmallBlockLargeHydrogenThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 30.0f },
                        { "MetalGrid", 22.0f },
                        { "LargeTube", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/SmallBlockSmallHydrogenThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                        { "Construction", 15.0f },
                        { "MetalGrid", 4.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 230.0f },
                        { "Construction", 60.0f },
                        { "LargeTube", 50.0f },
                        { "MetalGrid", 40.0f },
                        { "Motor", 1100.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/LargeBlockSmallAtmosphericThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 35.0f },
                        { "Construction", 50.0f },
                        { "LargeTube", 8.0f },
                        { "MetalGrid", 10.0f },
                        { "Motor", 110.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/SmallBlockLargeAtmosphericThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 4.0f },
                        { "MetalGrid", 8.0f },
                        { "Motor", 90.0f },
                    }
                },
                {
                    "MyObjectBuilder_Thrust/SmallBlockSmallAtmosphericThrust",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "Construction", 22.0f },
                        { "LargeTube", 1.0f },
                        { "MetalGrid", 1.0f },
                        { "Motor", 18.0f },
                    }
                },
                {
                    "MyObjectBuilder_Drill/SmallBlockDrill",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 32.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Drill/LargeBlockDrill",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 300.0f },
                        { "Construction", 40.0f },
                        { "LargeTube", 12.0f },
                        { "Motor", 5.0f },
                        { "Computer", 5.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipGrinder/LargeShipGrinder",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 1.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipGrinder/SmallShipGrinder",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "Construction", 17.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipWelder/LargeShipWelder",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 1.0f },
                        { "Motor", 2.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_ShipWelder/SmallShipWelder",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "Construction", 17.0f },
                        { "SmallTube", 6.0f },
                        { "Motor", 2.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_OreDetector/LargeOreDetector",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 50.0f },
                        { "Construction", 40.0f },
                        { "Motor", 5.0f },
                        { "Computer", 25.0f },
                        { "Detector", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_OreDetector/SmallBlockOreDetector",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 3.0f },
                        { "Construction", 2.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                        { "Detector", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_LandingGear/LargeBlockLandingGear",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 150.0f },
                        { "Construction", 20.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_LandingGear/SmallBlockLandingGear",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 5.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_JumpDrive/LargeJumpDrive",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 60.0f },
                        { "MetalGrid", 50.0f },
                        { "GravityGenerator", 20.0f },
                        { "Detector", 20.0f },
                        { "PowerCell", 120.0f },
                        { "Superconductor", 1000.0f },
                        { "Computer", 300.0f },
                        { "Construction", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_CameraBlock/SmallCameraBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Computer", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CameraBlock/LargeCameraBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Computer", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_MergeBlock/LargeShipMergeBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "Construction", 15.0f },
                        { "Motor", 2.0f },
                        { "LargeTube", 6.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MergeBlock/SmallShipMergeBlock",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 5.0f },
                        { "Motor", 1.0f },
                        { "SmallTube", 2.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Parachute/LgParachute",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 9.0f },
                        { "Construction", 25.0f },
                        { "SmallTube", 5.0f },
                        { "Motor", 3.0f },
                        { "Computer", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Parachute/SmParachute",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 2.0f },
                        { "SmallTube", 1.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Warhead/LargeWarhead",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Girder", 24.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 12.0f },
                        { "Computer", 2.0f },
                        { "Explosives", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Warhead/SmallWarhead",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Girder", 1.0f },
                        { "Construction", 1.0f },
                        { "SmallTube", 2.0f },
                        { "Computer", 1.0f },
                        { "Explosives", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Decoy/LargeDecoy",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 30.0f },
                        { "Construction", 10.0f },
                        { "Computer", 10.0f },
                        { "RadioCommunication", 1.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Decoy/SmallDecoy",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 1.0f },
                        { "Computer", 1.0f },
                        { "RadioCommunication", 1.0f },
                        { "SmallTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_LargeGatlingTurret/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 30.0f },
                        { "MetalGrid", 15.0f },
                        { "SmallTube", 6.0f },
                        { "Motor", 8.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_LargeGatlingTurret/SmallGatlingTurret",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "Construction", 30.0f },
                        { "MetalGrid", 5.0f },
                        { "SmallTube", 6.0f },
                        { "Motor", 4.0f },
                        { "Computer", 10.0f },
                    }
                },
                {
                    "MyObjectBuilder_LargeMissileTurret/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 20.0f },
                        { "Construction", 40.0f },
                        { "MetalGrid", 5.0f },
                        { "LargeTube", 6.0f },
                        { "Motor", 16.0f },
                        { "Computer", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_LargeMissileTurret/SmallMissileTurret",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 10.0f },
                        { "Construction", 40.0f },
                        { "MetalGrid", 2.0f },
                        { "LargeTube", 2.0f },
                        { "Motor", 8.0f },
                        { "Computer", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_InteriorTurret/LargeInteriorTurret",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 6.0f },
                        { "Construction", 20.0f },
                        { "SmallTube", 1.0f },
                        { "Motor", 2.0f },
                        { "Computer", 5.0f },
                        { "SteelPlate", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_SmallMissileLauncher/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 2.0f },
                        { "MetalGrid", 1.0f },
                        { "LargeTube", 4.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 35.0f },
                        { "Construction", 8.0f },
                        { "MetalGrid", 30.0f },
                        { "LargeTube", 25.0f },
                        { "Motor", 6.0f },
                        { "Computer", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_SmallMissileLauncherReload/SmallRocketLauncherReload",
                    new Dictionary<string, float>()
                    {
                        { "SmallTube", 50.0f },
                        { "InteriorPlate", 50.0f },
                        { "Construction", 24.0f },
                        { "LargeTube", 8.0f },
                        { "MetalGrid", 10.0f },
                        { "Motor", 4.0f },
                        { "Computer", 2.0f },
                        { "SteelPlate", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_SmallGatlingGun/(null)",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 4.0f },
                        { "Construction", 1.0f },
                        { "MetalGrid", 2.0f },
                        { "SmallTube", 6.0f },
                        { "Motor", 1.0f },
                        { "Computer", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/Suspension3x3",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 6.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/Suspension5x5",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 70.0f },
                        { "Construction", 40.0f },
                        { "LargeTube", 20.0f },
                        { "SmallTube", 30.0f },
                        { "Motor", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/Suspension1x1",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 6.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/SmallSuspension3x3",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 2.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/SmallSuspension5x5",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/SmallSuspension1x1",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 2.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/Suspension3x3mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 6.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/Suspension5x5mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 70.0f },
                        { "Construction", 40.0f },
                        { "LargeTube", 20.0f },
                        { "SmallTube", 30.0f },
                        { "Motor", 20.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/Suspension1x1mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 25.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 6.0f },
                        { "SmallTube", 12.0f },
                        { "Motor", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/SmallSuspension3x3mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 2.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/SmallSuspension5x5mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 4.0f },
                        { "Motor", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_MotorSuspension/SmallSuspension1x1mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 7.0f },
                        { "SmallTube", 2.0f },
                        { "Motor", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallRealWheel1x1",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 5.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallRealWheel",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallRealWheel5x5",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/RealWheel1x1",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 20.0f },
                        { "LargeTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/RealWheel",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "Construction", 25.0f },
                        { "LargeTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/RealWheel5x5",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallRealWheel1x1mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 5.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallRealWheelmirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallRealWheel5x5mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/RealWheel1x1mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 20.0f },
                        { "LargeTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/RealWheelmirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "Construction", 25.0f },
                        { "LargeTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/RealWheel5x5mirrored",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/Wheel1x1",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 8.0f },
                        { "Construction", 20.0f },
                        { "LargeTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallWheel1x1",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 2.0f },
                        { "Construction", 5.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/Wheel3x3",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 12.0f },
                        { "Construction", 25.0f },
                        { "LargeTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallWheel3x3",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 5.0f },
                        { "Construction", 10.0f },
                        { "LargeTube", 1.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/Wheel5x5",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 16.0f },
                        { "Construction", 30.0f },
                        { "LargeTube", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_Wheel/SmallWheel5x5",
                    new Dictionary<string, float>()
                    {
                        { "SteelPlate", 7.0f },
                        { "Construction", 15.0f },
                        { "LargeTube", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeWindowSquare",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 12.0f },
                        { "Construction", 8.0f },
                        { "SmallTube", 4.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/LargeWindowEdge",
                    new Dictionary<string, float>()
                    {
                        { "InteriorPlate", 16.0f },
                        { "Construction", 12.0f },
                        { "SmallTube", 6.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2Slope",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 16.0f },
                        { "BulletproofGlass", 55.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2Inv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 15.0f },
                        { "BulletproofGlass", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2Face",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 15.0f },
                        { "BulletproofGlass", 40.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2SideLeft",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 13.0f },
                        { "BulletproofGlass", 26.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2SideLeftInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 13.0f },
                        { "BulletproofGlass", 26.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2SideRight",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 13.0f },
                        { "BulletproofGlass", 26.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2SideRightInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 13.0f },
                        { "BulletproofGlass", 26.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1Slope",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 12.0f },
                        { "BulletproofGlass", 35.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1Face",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 11.0f },
                        { "BulletproofGlass", 24.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1Side",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 9.0f },
                        { "BulletproofGlass", 17.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1SideInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 9.0f },
                        { "BulletproofGlass", 17.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1Inv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 11.0f },
                        { "BulletproofGlass", 24.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 15.0f },
                        { "BulletproofGlass", 50.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x2FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 15.0f },
                        { "BulletproofGlass", 50.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 10.0f },
                        { "BulletproofGlass", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window1x1FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 10.0f },
                        { "BulletproofGlass", 25.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window3x3Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 40.0f },
                        { "BulletproofGlass", 196.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window3x3FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 40.0f },
                        { "BulletproofGlass", 196.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window2x3Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 25.0f },
                        { "BulletproofGlass", 140.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/Window2x3FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 25.0f },
                        { "BulletproofGlass", 140.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2Slope",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2Inv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2Face",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2SideLeft",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2SideLeftInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2SideRight",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2SideRightInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1Slope",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1Face",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1Side",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1SideInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1Inv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x2FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 3.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow1x1FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 1.0f },
                        { "BulletproofGlass", 2.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow3x3Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 3.0f },
                        { "BulletproofGlass", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow3x3FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 3.0f },
                        { "BulletproofGlass", 12.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow2x3Flat",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 2.0f },
                        { "BulletproofGlass", 8.0f },
                    }
                },
                {
                    "MyObjectBuilder_CubeBlock/SmallWindow2x3FlatInv",
                    new Dictionary<string, float>()
                    {
                        { "Girder", 2.0f },
                        { "BulletproofGlass", 8.0f },
                    }
                },

            };
        }

        public void QueueComponents()
        {
            // Ensure we don't increase the amount of parts on every subsequent run
            foreach (var component in neededComponents.Keys.ToList())
            {
                neededComponents[component] = 0.0f;
            }

            // Collect all components
            var remainingBlocks = projector.RemainingBlocksPerType;
            foreach (var pair in remainingBlocks)
            {
                Dictionary<string, float> components = componentsForBlock[pair.Key.ToString()];
                if (components != null)
                {
                    foreach (var component in components)
                    {
                        var key = CreateComponentBlueprint(component.Key);
                        if (key.HasValue)
                        {
                            neededComponents[key.Value] += component.Value * pair.Value;
                        }
                        else
                        {
                            Echo($"Could not create component blueprint for {component.Key}");
                            return;
                        }
                    }
                }
                else
                {
                    Echo($"Could not find components for {pair.Key.ToString()}");
                    return;
                }
            }

            // Queue all needed components
            foreach (var component in neededComponents.Keys)
            {
                if (neededComponents[component] > 0.0f)
                {
                    mainAssembler.AddQueueItem(component, neededComponents[component]); 
                }
            }
        }

        public MyDefinitionId? CreateComponentBlueprint(string name)
        {
            string postfix;
            switch (name)
            {
                case "SteelPlate":
                case "PowerCell":
                case "LargeTube":
                case "Display":
                case "MetalGrid":
                case "InteriorPlate":
                case "SmallTube":
                case "BulletproofGlass":
                case "Superconductor":
                case "SolarCell":
                    postfix = "";
                    break;
                case "RadioCommunication":
                case "Computer":
                case "Reactor":
                case "Detector":
                case "Construction":
                case "Thrust":
                case "Motor":
                case "Explosives":
                case "Girder":
                case "GravityGenerator":
                case "Medical":
                    postfix = "Component";
                    break;
                case "NATO_25x184mm":
                case "NATO_5p56x45mm":
                    postfix = "Magazine";
                    break;
                default:
                    postfix = "";
                    break;
            }

            MyDefinitionId id;
            if (MyDefinitionId.TryParse("MyObjectBuilder_BlueprintDefinition/" + name + postfix, out id) && (id.SubtypeId != null))
            {
                return id;
            }
            else
            {
                return null;
            }
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            QueueComponents();
        }
    }
}
