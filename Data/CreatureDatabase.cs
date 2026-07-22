using ArkPilot.Helpers;
using ArkPilot.Models;
using System;
using System.Collections.Generic;
using System.Linq;


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

                ["Ankylo-E"] = Create(
                        "Ankylo-E",
                        "Ankylo-E",
                        CreatureCategory.Herbivore,
                        aliases: new[]
                        {
                            "Ankylo-E",
                        }),



                ["Argentavis"] = Create(
                        "Argentavis",
                        "Argentavis",
                        CreatureCategory.Carnivore,
                        isFlyer: true,
                        aliases: new[]
                        {
                            "Argy",
                            "Argent"
                        }),

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

                ["Castoroides"] = Create(
                        "Castoroides",
                        "Castoroides",
                        CreatureCategory.Herbivore,
                        aliases: new[]
                        {
                            "Beaver"
                        }),

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

                ["HuskyWolf"] = Create(
                        "HuskyWolf",
                        "HuskyWolf",
                        CreatureCategory.Carnivore,
                        aliases: new[]
                        {
                            "HuskyWolf"
                        }),

                ["Doedicurus"] = Create(
                        "Doedicurus",
                        "Doedicurus",
                        CreatureCategory.Herbivore,
                        aliases: new[]
                        {
                            "Doed"
                        }),

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
                        CreatureCategory.Herbivore,
                        aliases: "Stag"),

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
                        CreatureCategory.Carnivore, "🦦",
                        isAquatic: true,
                        aliases: new[]
                        {
                            "Otter",
                            "Loutre"
                        }),

                ["NobleOtter"] = Create(
                        "NobleOtter",
                        "Noble Otter",
                        CreatureCategory.Carnivore, "🦦",
                        isAquatic: true,
                        aliases: new[]
                        {
                            "Noble Otter",
                            "Noble_Otter",
                            "Otter",
                            "Loutre"
                        }),

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
                        isFlyer: true,
                        aliases: new[]
                        {
                            "Ptero"
                        }),

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
                        CreatureCategory.Carnivore,"🦖",
                        aliases: new[]
                        {
                            "Tyrannosaurus",
                            "Tyrannosaurus Rex"
                        }),

                ["TekRex"] = Create(
                        "TekRex",
                        "Tek Rex",
                        CreatureCategory.Carnivore, "🤖",
                        isTek: true,
                        aliases: new[]
                        {
                            "Tek Rex"
                        }),

                ["X_Rex"] = Create(
                        "X_Rex",
                        "X-Rex",
                        CreatureCategory.Carnivore,
                        isXCreature: true,
                        aliases: new[]
                        {
                            "X Rex"
                        }),

                ["R_Rex"] = Create(
                        "R_Rex",
                        "R-Rex",
                        CreatureCategory.Carnivore,
                        isRCreature: true,
                        aliases: new[]
                        {
                            "R Rex"
                        }),

                ["BionicRex"] = Create(
                        "BionicRex",
                        "Bionic Rex",
                        CreatureCategory.Carnivore, "🤖",
                        isTek: true,
                        aliases: new[]
                        {
                            "Bionic Rex"
                        }),

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

                ["Triceratops"] = Create(
                        "Triceratops",
                        "Triceratops",
                        CreatureCategory.Herbivore,
                        aliases: new[]
                        {
                            "Trike"
                        }),

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

                ["Archelon_ASA"] = CreateWithArkClass(
                        "Archelon_ASA",
                        "Archelon",
                        CreatureCategory.Herbivore,
                        "Archelon_Character_BP_ASA",
                        isAquatic: true,
                        aliases: new[]
                        {
                            "Archelon"
                        }),

                ["Bison"] = Create(
                        "Bison",
                        "Bison",
                        CreatureCategory.Herbivore),

                ["Ceratosaurus_ASA"] = CreateWithArkClass(
                        "Ceratosaurus_ASA",
                        "Ceratosaurus",
                        CreatureCategory.Carnivore,
                        "Ceratosaurus_Character_BP_ASA",
                        aliases: new[]
                        {
                            "Ceratosaurus",
                            "Cerato"
                        }),

                ["Cherufe_ASEP"] = CreateWithArkClass(
                        "Cherufe_ASEP",
                        "Magmasaure",
                        CreatureCategory.Fantasy,
                        "Cherufe_Character_BP_ASEP",
                        isFantasy: true,
                        aliases: new[]
                        {
                            "Cherufe",
                            "Magmasaure"
                        }),

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

                ["Wyvern_Fire"] = Create(
                        "Wyvern_Fire",
                        "Wyvern de feu",
                        CreatureCategory.Fantasy, "🔥",
                        isFlyer: true,
                        isFantasy: true,
                        aliases: new[]
                        {
                            "Fire Wyvern"
                        }),

                ["Wyvern_Lightning"] = Create(
                        "Wyvern_Lightning",
                        "Wyvern de foudre",
                        CreatureCategory.Fantasy, "⚡",
                        isFlyer: true,
                        isFantasy: true,
                        aliases: new[]
                        {
                            "Lightning Wyvern"
                        }),

                ["Wyvern_Poison"] = Create(
                        "Wyvern_Poison",
                        "Wyvern de poison",
                        CreatureCategory.Fantasy, "☠️",
                        isFlyer: true,
                        isFantasy: true,
                        aliases: new[]
                        {
                            "Poison Wyvern"
                        }),

                ["Wyvern_Ice"] = Create(
                        "Wyvern_Ice",
                        "Wyvern de glace",
                        CreatureCategory.Fantasy, "❄️",
                        isFlyer: true,
                        isFantasy: true,
                        aliases: new[]
                        {
                            "Ice Wyvern"
                        }),

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

                ["DungBeetle"] = Create(
                        "DungBeetle",
                        "Dung Beetle",
                        CreatureCategory.Insect,
                        aliases: "Scarab"),

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

                ["Raft_BP"] = Create(
                        "Raft_BP",
                        "Radeau",
                        CreatureCategory.Structure,
                        "🛶",
                        "Raft",
                        "Radeau"),

                ["TekParasaur"] = Create(
                        "TekParasaur",
                        "Tek Parasaur",
                        CreatureCategory.Herbivore,
                        isTek: true),

                ["TekRaptor"] = Create(
                        "TekRaptor",
                        "Tek Raptor",
                        CreatureCategory.Carnivore,
                        isTek: true),

                ["TekStego"] = Create(
                        "TekStego",
                        "Tek Stegosaurus",
                        CreatureCategory.Herbivore,
                        isTek: true),

                ["TekTrike"] = Create(
                        "TekTrike",
                        "Tek Triceratops",
                        CreatureCategory.Herbivore,
                        isTek: true),

                ["TekRex"] = Create(
                        "TekRex",
                        "Tek Rex",
                        CreatureCategory.Carnivore,
                        isTek: true),

                ["TekQuetzal"] = Create(
                        "TekQuetzal",
                        "Tek Quetzal",
                        CreatureCategory.Flyer,
                        isTek: true),

            };

        internal static int Count =>
    Creatures.Count;

        internal static CreatureInfo? Find(
    string species)
        {
            if (string.IsNullOrWhiteSpace(species))
            {
                return null;
            }

            string search = species.Trim();

            if (Creatures.TryGetValue(
                search,
                out CreatureInfo? creature))
            {
                return creature;
            }

            return Creatures.Values
                .FirstOrDefault(c =>
                    c.Species.Equals(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    c.DisplayName.Equals(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                    c.Aliases.Any(alias =>
                        alias.Equals(
                            search,
                            StringComparison.OrdinalIgnoreCase)));
        }


        internal static CreatureInfo Create(
    string species,
    string displayName,
    CreatureCategory category,
    params string[] aliases)
        {
            return Create(
                species,
                displayName,
                category,
                "",
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                aliases);
        }


        internal static CreatureInfo Create(
    string species,
    string displayName,
    CreatureCategory category,
    string icon,
    params string[] aliases)
        {
            return Create(
                species,
                displayName,
                category,
                icon,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                aliases);
        }


        internal static CreatureInfo CreateWithArkClass(
                string species,
                string displayName,
                CreatureCategory category,
                string arkClass,
                string icon = "",
                bool isTek = false,
                bool isBoss = false,
                bool isFlyer = false,
                bool isAquatic = false,
                bool isAberrant = false,
                bool isXCreature = false,
                bool isRCreature = false,
                bool isFantasy = false,
                params string[] aliases)
            {
                return new CreatureInfo
                {
                    Species = species,
                    ArkClass = arkClass,
                    DisplayName = displayName,
                    Aliases = aliases,
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



        internal static CreatureInfo Create(
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
            bool isFantasy = false,
            params string[] aliases)
        {
            return new CreatureInfo
            {
                Species = species,
                ArkClass = species,
                DisplayName = displayName,
                Aliases = aliases,
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

        public static IReadOnlyCollection<CreatureInfo> GetAll()
        {
            return Creatures.Values;
        }

        public static CreatureInfo? GetBySpecies(
            string species)
        {
            if (string.IsNullOrWhiteSpace(species))
            {
                return null;
            }

            Creatures.TryGetValue(
                species,
                out var creature);

            return creature;
        }

        public static IEnumerable<CreatureInfo> GetByCategory(
            CreatureCategory category)
        {
            return Creatures.Values
                .Where(c => c.Category == category);
        }

        public static IEnumerable<CreatureInfo> GetFlyers()
        {
            return Creatures.Values
                .Where(c => c.IsFlyer);
        }

        public static IEnumerable<CreatureInfo> GetAquatics()
        {
            return Creatures.Values
                .Where(c => c.IsAquatic);
        }

        public static IEnumerable<CreatureInfo> GetTekCreatures()
        {
            return Creatures.Values
                .Where(c => c.IsTek);
        }

        public static IEnumerable<CreatureInfo> GetBosses()
        {
            return Creatures.Values
                .Where(c => c.IsBoss);
        }

        public static IEnumerable<CreatureInfo> GetFantasyCreatures()
        {
            return Creatures.Values
                .Where(c => c.IsFantasy);
        }

        public static CreatureInfo? GetByClassName(string? className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return null;

            string value = className.Trim();

            // Retire le chemin ARK.
            int lastSlash = value.LastIndexOf('/');

            if (lastSlash >= 0)
                value = value[(lastSlash + 1)..];

            // Exemple :
            // Raptor_Character_BP.Raptor_Character_BP_C
            // devient :
            // Raptor_Character_BP_C
            int lastDot = value.LastIndexOf('.');

            if (lastDot >= 0)
                value = value[(lastDot + 1)..];

            string arkClass = value;

            if (arkClass.EndsWith(
                    "_C",
                    StringComparison.OrdinalIgnoreCase))
            {
                arkClass = arkClass[..^2];
            }

            var arkClassMatch =
                Creatures.Values.FirstOrDefault(c =>
                    c.ArkClass.Equals(
                        arkClass,
                        StringComparison.OrdinalIgnoreCase));

            if (arkClassMatch != null)
            {
                return arkClassMatch;
            }

            const string tekSuffix =
                "_Character_BP_Tek_C";

            const string normalSuffix =
                "_Character_BP_C";

            if (value.EndsWith(
                    tekSuffix,
                    StringComparison.OrdinalIgnoreCase))
            {
                value =
                    value[..^tekSuffix.Length];

                value =
                    "Tek" + value;
            }
            else if (value.EndsWith(
                         normalSuffix,
                         StringComparison.OrdinalIgnoreCase))
            {
                value =
                    value[..^normalSuffix.Length];
            }

            value = value
                .Replace("_Character_BP_ASA", "")
                .Replace("_Character_BP_ASEP", "")
                .Replace("_Character_BP", "")
                .Replace("_BP", "");

            var creature = Find(value);

            if (creature != null)
            {
                return creature;
            }

            // Recherche plus souple pour les variantes ASA / mods
            return Creatures.Values.FirstOrDefault(c =>
                value.Contains(
                    c.Species,
                    StringComparison.OrdinalIgnoreCase));
        }
    }
}