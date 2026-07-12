# Changelog

All notable changes to this project will be documented in this file.

---

# [12.6.1] - 2026-07

## 🐛 Maintenance Release

### Fixed

- Fixed crash when opening the Dashboard after saving server settings
- Fixed ServerMonitor initialization after configuration changes
- Removed repetitive Nitrado status polling logs
- Removed automatic `ListPlayers` spam from the RCON console
- Fixed duplicate RCON console command output
- Fixed in-game server messages
- Added `ADMIN:` prefix to server chat messages
- Shared BackupService state across Dashboard, Server and Backup pages
- Fixed last SaveWorld time synchronization
- Fixed full backup being limited to Ragnarok
- Added active map detection through the Nitrado API
- Fixed FTP backup selection when multiple map folders are present
- Fixed Dashboard online/offline badge color

# [12.6.0] - 2026-07

## 🎉 First Public Release

### Added

- New ArkPilot branding
- Modern dashboard
- Complete RCON management
- FTP Explorer
- Backup Manager
- Complete world backup
- ZIP backup creation
- Live event log
- Players management
- Nitrado API integration
- Settings page
- About window
- Official application icon

### Improved

- Completely redesigned interface
- Cleaner project architecture
- Better service separation
- Improved code organization

### Fixed

- Multiple RCON stability improvements
- FTP transfer reliability
- Backup creation improvements
- Various UI fixes