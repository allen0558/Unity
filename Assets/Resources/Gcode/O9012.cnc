O9012
G54G90G00X[#3]Y0Z100
S500M3
G01Z0F300
WHILE[#1LE10]DO1
#7=#1/TAN[#5]+#3
G1Z-#1X#7
#8=#6/2-ROUND[#6/2]
IF[#8EQ0]GOTO10
G1Y0
GOTO20
N10Y#4
N20#1=#1+#2
#6=#6+1
END1
G0
Z100