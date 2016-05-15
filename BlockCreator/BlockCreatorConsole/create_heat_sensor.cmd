@echo off

block module --create -n "Quest Institute" -v "1.00"

block block --create -n "Light" -f "Action"

block param --create -n "State" -d "Input" --defaultValue "1" --minValue "0" --maxValue "1"

block hardware -b "Light" -d "Input"

block mode --create -b "Light" -n "Set" -v "LightSet.vix" -p "State" -t "Change"

block finish