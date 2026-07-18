using ArkPilot.Helpers;
using ArkPilot.Models;
using System;
using System.Collections.Generic;

namespace ArkPilot.Data
{
    internal static class CreatureDatabase
    {
        internal static readonly Dictionary<string, CreatureInfo> Creatures =
            new(StringComparer.OrdinalIgnoreCase)
            {

                ["Toad"] = Create(
                        "Toad",
                        "Beelzebufo",
                        CreatureCategory.Carnivore),

                ["Ankylo"] = Create(
                        "Ankylo",
                        "Ankylosaurus",
                        CreatureCategory.Herbivore),

                ["Argent"] = Create(
                        "Argent",
                        "Argentavis",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Basilisk"] = Create(
                        "Basilisk",
                        "Basilisk",
                        CreatureCategory.Fantasy,
                        isFantasy: true),

                ["Basilosaurus"] = Create(
                        "Basilosaurus",
                        "Basilosaurus",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Beaver"] = Create(
                        "Beaver",
                        "Castoroides",
                        CreatureCategory.Herbivore),

                ["Bigfoot"] = Create(
                        "Bigfoot",
                        "Gigantopithecus",
                        CreatureCategory.Herbivore),

                ["Bronto"] = Create(
                        "Bronto",
                        "Brontosaurus",
                        CreatureCategory.Herbivore),

                ["Carno"] = Create(
                        "Carno",
                        "Carnotaurus",
                        CreatureCategory.Carnivore),

                ["Chalico"] = Create(
                        "Chalico",
                        "Chalicotherium",
                        CreatureCategory.Herbivore),

                ["Compy"] = Create(
                        "Compy",
                        "Compy",
                        CreatureCategory.Carnivore),

                ["Daeodon"] = Create(
                        "Daeodon",
                        "Daeodon",
                        CreatureCategory.Omnivore),

                ["Direbear"] = Create(
                        "Direbear",
                        "Dire Bear",
                        CreatureCategory.Omnivore),

                ["Direwolf"] = Create(
                        "Direwolf",
                        "Direwolf",
                        CreatureCategory.Carnivore),

                ["Doed"] = Create(
                        "Doed",
                        "Doedicurus",
                        CreatureCategory.Herbivore),

                ["Dodo"] = Create(
                        "Dodo",
                        "Dodo",
                        CreatureCategory.Herbivore),

                ["Dolphin"] = Create(
                        "Dolphin",
                        "Ichthyosaurus",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Equus"] = Create(
                        "Equus",
                        "Equus",
                        CreatureCategory.Herbivore),

                ["Galli"] = Create(
                        "Galli",
                        "Gallimimus",
                        CreatureCategory.Herbivore),

                ["Giga"] = Create(
                        "Giga",
                        "Giganotosaurus",
                        CreatureCategory.Carnivore),

                ["GigantRaptor"] = Create(
                        "GigantRaptor",
                        "Gigantoraptor",
                        CreatureCategory.Omnivore),

                ["Griffin"] = Create(
                        "Griffin",
                        "Griffin",
                        CreatureCategory.Fantasy,
                        isFlyer: true,
                        isFantasy: true),

                ["Iguanodon"] = Create(
                        "Iguanodon",
                        "Iguanodon",
                        CreatureCategory.Herbivore),

                ["Kapro"] = Create(
                        "Kapro",
                        "Kaprosuchus",
                        CreatureCategory.Carnivore),

                ["Kentrosaurus"] = Create(
                        "Kentrosaurus",
                        "Kentrosaurus",
                        CreatureCategory.Herbivore),

                ["Lystro"] = Create(
                        "Lystro",
                        "Lystrosaurus",
                        CreatureCategory.Herbivore),

                ["Mammoth"] = Create(
                        "Mammoth",
                        "Mammoth",
                        CreatureCategory.Herbivore),

                ["Manta"] = Create(
                        "Manta",
                        "Manta",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Megachelon"] = Create(
                        "Megachelon",
                        "Megachelon",
                        CreatureCategory.Herbivore,
                        isAquatic: true),

                ["Megalania"] = Create(
                        "Megalania",
                        "Megalania",
                        CreatureCategory.Carnivore),

                ["Megaloceros"] = Create(
                        "Megaloceros",
                        "Megaloceros",
                        CreatureCategory.Herbivore),

                ["Megalodon"] = Create(
                        "Megalodon",
                        "Megalodon",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Megalosaurus"] = Create(
                        "Megalosaurus",
                        "Megalosaurus",
                        CreatureCategory.Carnivore),

                ["Megatherium"] = Create(
                        "Megatherium",
                        "Megatherium",
                        CreatureCategory.Omnivore),

                ["Mesopithecus"] = Create(
                        "Mesopithecus",
                        "Mesopithecus",
                        CreatureCategory.Omnivore),

                ["Moschops"] = Create(
                        "Moschops",
                        "Moschops",
                        CreatureCategory.Omnivore),

                ["Mosa"] = Create(
                        "Mosa",
                        "Mosasaurus",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Otter"] = Create(
                        "Otter",
                        "Otter",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Oviraptor"] = Create(
                        "Oviraptor",
                        "Oviraptor",
                        CreatureCategory.Omnivore),

                ["Pachy"] = Create(
                        "Pachy",
                        "Pachycephalosaurus",
                        CreatureCategory.Herbivore),

                ["Pachyrhino"] = Create(
                        "Pachyrhino",
                        "Pachyrhinosaurus",
                        CreatureCategory.Herbivore),

                ["Parasaur"] = Create(
                        "Parasaur",
                        "Parasaur",
                        CreatureCategory.Herbivore),

                ["Pegomastax"] = Create(
                        "Pegomastax",
                        "Pegomastax",
                        CreatureCategory.Herbivore),

                ["Pelagornis"] = Create(
                        "Pelagornis",
                        "Pelagornis",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Phiomia"] = Create(
                        "Phiomia",
                        "Phiomia",
                        CreatureCategory.Herbivore),

                ["Plesiosaur"] = Create(
                        "Plesiosaur",
                        "Plesiosaur",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Procoptodon"] = Create(
                        "Procoptodon",
                        "Procoptodon",
                        CreatureCategory.Herbivore),

                ["Pteranodon"] = Create(
                        "Pteranodon",
                        "Pteranodon",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Pulmonoscorpius"] = Create(
                        "Pulmonoscorpius",
                        "Pulmonoscorpius",
                        CreatureCategory.Insect),

                ["Quetz"] = Create(
                        "Quetz",
                        "Quetzal",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Raptor"] = Create(
                        "Raptor",
                        "Raptor",
                        CreatureCategory.Carnivore),

                ["Rex"] = Create(
                        "Rex",
                        "Rex",
                        CreatureCategory.Carnivore),

                ["Sabertooth"] = Create(
                        "Sabertooth",
                        "Sabertooth",
                        CreatureCategory.Carnivore),

                ["Sarco"] = Create(
                        "Sarco",
                        "Sarcosuchus",
                        CreatureCategory.Carnivore),

                ["Spino"] = Create(
                        "Spino",
                        "Spinosaurus",
                        CreatureCategory.Carnivore),

                ["Stego"] = Create(
                        "Stego",
                        "Stegosaurus",
                        CreatureCategory.Herbivore),

                ["Tapejara"] = Create(
                        "Tapejara",
                        "Tapejara",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Therizino"] = Create(
                        "Therizino",
                        "Therizinosaur",
                        CreatureCategory.Herbivore),

                ["Titanoboa"] = Create(
                        "Titanoboa",
                        "Titanoboa",
                        CreatureCategory.Carnivore),

                ["Trike"] = Create(
                        "Trike",
                        "Triceratops",
                        CreatureCategory.Herbivore),

                ["Tusoteuthis"] = Create(
                        "Tusoteuthis",
                        "Tusoteuthis",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["WoollyRhino"] = Create(
                        "WoollyRhino",
                        "Woolly Rhino",
                        CreatureCategory.Herbivore),

                ["Yutyrannus"] = Create(
                        "Yutyrannus",
                        "Yutyrannus",
                        CreatureCategory.Carnivore),

                ["Allo"] = Create(
                        "Allo",
                        "Allosaurus",
                        CreatureCategory.Carnivore),

                ["Ammonite"] = Create(
                        "Ammonite",
                        "Ammonite",
                        CreatureCategory.Aquatic,
                        isAquatic: true),

                ["Angler"] = Create(
                        "Angler",
                        "Anglerfish",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["SpiderS"] = Create(
                        "SpiderS",
                        "Araneo",
                        CreatureCategory.Insect),

                ["Archa"] = Create(
                        "Archa",
                        "Archaeopteryx",
                        CreatureCategory.Omnivore,
                        isFlyer: true),

                ["Arthro"] = Create(
                        "Arthro",
                        "Arthropluera",
                        CreatureCategory.Insect),

                ["Baryonyx"] = Create(
                        "Baryonyx",
                        "Baryonyx",
                        CreatureCategory.Carnivore),

                ["Bat"] = Create(
                        "Bat",
                        "Onyc",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Bee"] = Create(
                        "Bee",
                        "Giant Bee",
                        CreatureCategory.Insect,
                        isFlyer: true),

                ["Dimetro"] = Create(
                        "Dimetro",
                        "Dimetrodon",
                        CreatureCategory.Carnivore),

                ["Dimorph"] = Create(
                        "Dimorph",
                        "Dimorphodon",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Dunkle"] = Create(
                        "Dunkle",
                        "Dunkleosteus",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Electrophorus"] = Create(
                        "Electrophorus",
                        "Electrophorus",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Euryp"] = Create(
                        "Euryp",
                        "Eurypterid",
                        CreatureCategory.Aquatic,
                        isAquatic: true),

                ["Hesperornis"] = Create(
                        "Hesperornis",
                        "Hesperornis",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Hyaenodon"] = Create(
                        "Hyaenodon",
                        "Hyaenodon",
                        CreatureCategory.Carnivore),

                ["Icthyornis"] = Create(
                        "Icthyornis",
                        "Ichthyornis",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Leech"] = Create(
                        "Leech",
                        "Leech",
                        CreatureCategory.Aquatic,
                        isAquatic: true),

                ["Liopleurodon"] = Create(
                        "Liopleurodon",
                        "Liopleurodon",
                        CreatureCategory.Carnivore,
                        isAquatic: true),

                ["Mantis"] = Create(
                        "Mantis",
                        "Mantis",
                        CreatureCategory.Insect),

                ["Microraptor"] = Create(
                        "Microraptor",
                        "Microraptor",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Purlovia"] = Create(
                        "Purlovia",
                        "Purlovia",
                        CreatureCategory.Carnivore),

                ["Rhino"] = Create(
                        "Rhino",
                        "Woolly Rhino",
                        CreatureCategory.Herbivore),

                ["Thylacoleo"] = Create(
                        "Thylacoleo",
                        "Thylacoleo",
                        CreatureCategory.Carnivore),

                ["Deathworm"] = Create(
                        "Deathworm",
                        "Deathworm",
                        CreatureCategory.Insect),

                ["Jerboa"] = Create(
                        "Jerboa",
                        "Jerboa",
                        CreatureCategory.Herbivore),

                ["Moth"] = Create(
                        "Moth",
                        "Lymantria",
                        CreatureCategory.Insect,
                        isFlyer: true),

                ["Camelsaurus"] = Create(
                        "Camelsaurus",
                        "Morellatops",
                        CreatureCategory.Herbivore),

                ["Phoenix"] = Create(
                        "Phoenix",
                        "Phoenix",
                        CreatureCategory.Fantasy,
                        isFlyer: true,
                        isFantasy: true),

                ["RockGolem"] = Create(
                        "RockGolem",
                        "Rock Elemental",
                        CreatureCategory.Fantasy,
                        isFantasy: true),

                ["SpineyLizard"] = Create(
                        "SpineyLizard",
                        "Thorny Dragon",
                        CreatureCategory.Herbivore),

                ["Vulture"] = Create(
                        "Vulture",
                        "Vulture",
                        CreatureCategory.Carnivore,
                        isFlyer: true),

                ["Wyvern"] = Create(
                        "Wyvern",
                        "Wyvern",
                        CreatureCategory.Fantasy,
                        isFlyer: true,
                        isFantasy: true),

                ["Manticore"] = Create(
                        "Manticore",
                        "Manticore",
                        CreatureCategory.Boss,
                        isBoss: true,
                        isFlyer: true,
                        isFantasy: true),

                ["LanternPug"] = Create(
                        "LanternPug",
                        "Bulbdog",
                        CreatureCategory.Omnivore,
                        isFantasy: true),

                ["LanternBird"] = Create(
                        "LanternBird",
                        "Featherlight",
                        CreatureCategory.Omnivore,
                        isFlyer: true,
                        isFantasy: true),

                ["LanternLizard"] = Create(
                        "LanternLizard",
                        "Glowtail",
                        CreatureCategory.Omnivore,
                        isFantasy: true),

                ["LanternGoat"] = Create(
                        "LanternGoat",
                        "Shinehorn",
                        CreatureCategory.Herbivore,
                        isFantasy: true),

                ["CaveWolf"] = Create(
                        "CaveWolf",
                        "Ravager",
                        CreatureCategory.Carnivore,
                        isFantasy: true),

                ["MoleRat"] = Create(
                        "MoleRat",
                        "Roll Rat",
                        CreatureCategory.Herbivore,
                        isFantasy: true),

                ["Crab"] = Create(
                        "Crab",
                        "Karkinos",
                        CreatureCategory.Carnivore,
                        isFantasy: true),

                ["RockDrake"] = Create(
                        "RockDrake",
                        "Rock Drake",
                        CreatureCategory.Fantasy,
                        isFantasy: true),

                ["Nameless"] = Create(
                        "Nameless",
                        "Nameless",
                        CreatureCategory.Fantasy,
                        isFantasy: true),

                ["Seeker"] = Create(
                        "Seeker",
                        "Seeker",
                        CreatureCategory.Fantasy,
                        isFlyer: true,
                        isFantasy: true),

                ["Lamprey"] = Create(
                        "Lamprey",
                        "Lamprey",
                        CreatureCategory.Aquatic,
                        isAquatic: true,
                        isFantasy: true),

                ["GlowBug"] = Create(
                        "GlowBug",
                        "Glowbug",
                        CreatureCategory.Insect,
                        isFlyer: true,
                        isFantasy: true),

                ["ReaperKing"] = Create(
                        "ReaperKing",
                        "Reaper King",
                        CreatureCategory.Fantasy,
                        isFantasy: true),

                ["ReaperQueen"] = Create(
                        "ReaperQueen",
                        "Reaper Queen",
                        CreatureCategory.Fantasy,
                        isFantasy: true),

                ["Rockwell"] = Create(
                        "Rockwell",
                        "Rockwell",
                        CreatureCategory.Boss,
                        isBoss: true,
                        isFantasy: true),

                ["YiLing"] = Create(
                        "YiLing",
                        "Yi Ling",
                        CreatureCategory.Carnivore,
                        isFantasy: true),

                ["Gacha"] = Create(
                        "Gacha",
                        "Gacha",
                        CreatureCategory.Omnivore,
                        isFantasy: true),

                ["GasBags"] = Create(
                        "GasBags",
                        "Gasbags",
                        CreatureCategory.Herbivore,
                        isFantasy: true),

                ["IceJumper"] = Create(
                        "IceJumper",
                        "Managarmr",
                        CreatureCategory.Carnivore,
                        isFantasy: true),

                ["Owl"] = Create(
                        "Owl",
                        "Snow Owl",
                        CreatureCategory.Carnivore,
                        isFlyer: true,
                        isFantasy: true),

                ["Spindles"] = Create(
                        "Spindles",
                        "Velonasaur",
                        CreatureCategory.Carnivore,
                        isFantasy: true),

                ["Enforcer"] = Create(
                        "Enforcer",
                        "Enforcer",
                        CreatureCategory.Fantasy,
                        isTek: true,
                        isFantasy: true),

                ["Scout"] = Create(
                        "Scout",
                        "Scout",
                        CreatureCategory.Flyer,
                        isTek: true,
                        isFlyer: true,
                        isFantasy: true),

                ["Mek"] = Create(
                        "Mek",
                        "Mek",
                        CreatureCategory.Fantasy,
                        isTek: true,
                        isFantasy: true),

                ["DesertKaiju"] = Create(
                        "DesertKaiju",
                        "Desert Titan",
                        CreatureCategory.Boss,
                        isBoss: true,
                        isFlyer: true,
                        isFantasy: true),

                ["ForestKaiju"] = Create(
                        "ForestKaiju",
                        "Forest Titan",
                        CreatureCategory.Boss,
                        isBoss: true,
                        isFantasy: true),

                ["IceKaiju"] = Create(
                        "IceKaiju",
                        "Ice Titan",
                        CreatureCategory.Boss,
                        isBoss: true,
                        isFantasy: true),

                ["KingKaiju"] = Create(
                        "KingKaiju",
                        "King Titan",
                        CreatureCategory.Boss,
                        isBoss: true,
                        isFantasy: true),

            };

        internal static int Count =>
    Creatures.Count;

        private static CreatureInfo Create(
            string species,
            string displayName,
            CreatureCategory category,
            string icon = "",
            bool isTek = false,
            bool isBoss = false,
            bool isFlyer = false,
            bool isAquatic = false,
            bool isAberrant = false,
            bool isXCreature = false,
            bool isRCreature = false,
            bool isFantasy = false)
        {
            return new CreatureInfo
            {
                Species = species,
                DisplayName = displayName,
                Icon = icon,
                Category = category,
                IsTek = isTek,
                IsBoss = isBoss,
                IsFlyer = isFlyer,
                IsAquatic = isAquatic,
                IsAberrant = isAberrant,
                IsXCreature = isXCreature,
                IsRCreature = isRCreature,
                IsFantasy = isFantasy
            };
        }
    }
}