#!/bin/bash

version=`cat version.txt`
build=$[`git log --oneline | wc -l`]
builddate=`date +"%F" `
buildtime=`date +"%T" `
commithash=`git show -s --format=%H`

echo "Generating build $build ($builddate $buildtime)..."

# For .NET Core projects
echo "<!-- Auto-generated build information code file -->" > Directory.Build.props
echo "<!-- (C) Xlfdll Workstation -->" >> Directory.Build.props
echo "" >> Directory.Build.props
echo "<Project>" >> Directory.Build.props
echo "  <PropertyGroup>" >> Directory.Build.props
echo "    <Version>$version.$build</Version>" >> Directory.Build.props
echo "  </PropertyGroup>" >> Directory.Build.props
echo "</Project>" >> Directory.Build.props

echo "Done."