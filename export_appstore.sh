#!/bin/bash

set -e

echo "=== APP STORE EXPORT START ==="
echo "XCARCHIVE_PATH: $XCARCHIVE_PATH"
echo "OUTPUT_DIRECTORY: $OUTPUT_DIRECTORY"

EXPORT_DIR="$OUTPUT_DIRECTORY/AppStoreExport"
mkdir -p "$EXPORT_DIR"

EXPORT_OPTIONS="$EXPORT_DIR/ExportOptions.plist"

cat > "$EXPORT_OPTIONS" <<'PLIST'
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>method</key>
    <string>app-store</string>
    <key>generateAppStoreInformation</key>
    <true/>
</dict>
</plist>
PLIST

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