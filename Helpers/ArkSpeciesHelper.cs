using System;
using System.Collections.Generic;

namespace ArkPilot.Helpers
{
    public static class ArkSpeciesHelper
    {
        private static readonly Dictionary<string, string> _species =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Toad"] = "Beelzebufo",
                ["Ankylo"] = "Ankylosaurus",
                ["Argent"] = "Argentavis",
                ["Basilisk"] = "Basilisk",
                ["Basilosaurus"] = "Basilosaurus",
                ["Beaver"] = "Castoroides",
                ["Bigfoot"] = "Gigantopithecus",
                ["Bronto"] = "Brontosaurus",
                ["Carno"] = "Carnotaurus",
                ["Chalico"] = "Chalicotherium",
                ["Compy"] = "Compy",
                ["Daeodon"] = "Daeodon",
                ["Direbear"] = "Dire Bear",
                ["Direwolf"] = "Direwolf",
                ["Doed"] = "Doedicurus",
                ["Dolphin"] = "Ichthyosaurus",
                ["Dragonfly"] = "Meganeura",
                ["DungBeetle"] = "Dung Beetle",
                ["Equus"] = "Equus",
                ["Galli"] = "Gallimimus",
                ["Giga"] = "Giganotosaurus",
                ["Gigant"] = "Gigantopithecus",
                ["Hyaenodon"] = "Hyaenodon",
                ["Ichthyornis"] = "Ichthyornis",
                ["Kairuku"] = "Kairuku",
                ["Kaprosuchus"] = "Kaprosuchus",
                ["Kentro"] = "Kentrosaurus",
                ["Lystro"] = "Lystrosaurus",
                ["Mammoth"] = "Mammoth",
                ["Manta"] = "Manta",
                ["Megalania"] = "Megalania",
                ["Megaloceros"] = "Megaloceros",
                ["Megalodon"] = "Megalodon",
                ["Megalosaurus"] = "Megalosaurus",
                ["Megatherium"] = "Megatherium",
                ["Mesopithecus"] = "Mesopithecus",
                ["Mosasaur"] = "Mosasaurus",
                ["Otter"] = "Otter",
                ["Oviraptor"] = "Oviraptor",
                ["Pachy"] = "Pachycephalosaurus",
                ["Pachyrhino"] = "Pachyrhinosaurus",
                ["Paracer"] = "Paraceratherium",
                ["Parasaur"] = "Parasaur",
                ["Pelagornis"] = "Pelagornis",
                ["Phiomia"] = "Phiomia",
                ["Plesiosaur"] = "Plesiosaurus",
                ["Procoptodon"] = "Procoptodon",
                ["Ptera"] = "Pteranodon",
                ["Quetz"] = "Quetzal",
                ["Raptor"] = "Raptor",
                ["Rex"] = "Rex",
                ["Rhyniognatha"] = "Rhyniognatha",
                ["Sabertooth"] = "Sabertooth",
                ["Sarco"] = "Sarcosuchus",
                ["Spino"] = "Spinosaurus",
                ["SpiderS"] = "Araneo",
                ["Stego"] = "Stegosaurus",
                ["Tapejara"] = "Tapejara",
                ["TerrorBird"] = "Terror Bird",
                ["Therizino"] = "Therizinosaurus",
                ["Thyla"] = "Thylacoleo",
                ["Titanoboa"] = "Titanoboa",
                ["Trike"] = "Triceratops",
                ["Troodon"] = "Troodon",
                ["Tuso"] = "Tusoteuthis",
                ["Yuty"] = "Yutyrannus"
            };

        public static string GetDisplayName(string species)
        {
            if (_species.TryGetValue(species, out var displayName))
            {
                return displayName;
            }

            return species;
        }
    }
}