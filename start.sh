if [ $1 ]; then

java -classpath $1 server.logic.MonoServer

else
	echo "unkown class path\nusage ./start.sh [class path]"
fi
