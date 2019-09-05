if [ $1 ]; then
outdir=$1
rm -rf $outdir
mkdir $outdir
filelist=`ls src/server/logic/*.java`
for file in $filelist;do
	echo "build $file"
	javac -classpath src $file -d $outdir
done

cp src/server/configuration.properties $outdir/server/configuration.properties
else
echo "no out put dir, usage\n ./build.sh [out dir]\n"
fi

