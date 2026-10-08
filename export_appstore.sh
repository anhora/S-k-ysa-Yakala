#!/bin/bash

set -e

echo "=== APP STORE EXPORT START ==="
echo "XCARCHIVE_PATH: $XCARCHIVE_PATH"
echo "OUTPUT_DIRECTORY: $OUTPUT_DIRECTORY"

EXPORT_DIR="$OUTPUT_DIRECTORY/AppStoreExport"
mkdir -p "$EXPORT_DIR"

APP_PATH="$XCARCHIVE_PATH/Products/Applications/SkysaYakala.app"
PROFILE="$APP_PATH/embedded.mobileprovision"

echo "=== READING PROVISIONING PROFILE ==="
echo "PROFILE: $PROFILE"

if [ ! -f "$PROFILE" ]; then
    echo "ERROR: embedded.mobileprovision NOT FOUND"
    exit 1
fi

security cms -D -i "$PROFILE" > "$EXPORT_DIR/profile.plist"

PROFILE_NAME=$(/usr/libexec/PlistBuddy -c "Print :Name" "$EXPORT_DIR/profile.plist")
PROFILE_UUID=$(/usr/libexec/PlistBuddy -c "Print :UUID" "$EXPORT_DIR/profile.plist")
PROFILE_APP_ID=$(/usr/libexec/PlistBuddy -c "Print :Entitlements:application-identifier" "$EXPORT_DIR/profile.plist")

echo "PROFILE NAME: $PROFILE_NAME"
echo "PROFILE UUID: $PROFILE_UUID"
echo "PROFILE APP ID: $PROFILE_APP_ID"

EXPORT_OPTIONS="$EXPORT_DIR/ExportOptions.plist"

cat > "$EXPORT_OPTIONS" <<PLIST
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>method</key>
    <string>app-store-connect</string>

    <key>signingStyle</key>
    <string>manual</string>

    <key>teamID</key>
    <string>337ZS9K4W4</string>

    <key>provisioningProfiles</key>
    <dict>
        <key>com.abdullahmermer.sikiysayakala</key>
        <string>$PROFILE_NAME</string>
    </dict>

    <key>generateAppStoreInformation</key>
    <true/>
</dict>
</plist>
PLIST

echo "=== EXPORT OPTIONS ==="
cat "$EXPORT_OPTIONS"

echo "=== EXPORTING XCARCHIVE ==="

xcodebuild \
    -exportArchive \
    -archivePath "$XCARCHIVE_PATH" \
    -exportPath "$EXPORT_DIR" \
    -exportOptionsPlist "$EXPORT_OPTIONS"

echo "=== SEARCHING FOR APPSTOREINFO ==="

find "$EXPORT_DIR" -name "AppStoreInfo.plist" -print

if [ -f "$EXPORT_DIR/AppStoreInfo.plist" ]; then
    echo "=== AppStoreInfo.plist FOUND ==="
else
    echo "ERROR: AppStoreInfo.plist NOT FOUND"
    exit 1
fi

echo "=== APP STORE EXPORT COMPLETE ==="