while getopts ":t:" opt; do
  case $opt in
    t) var="$OPTARG";;
    \?) echo "nothing happen: -$OPTAR·G" >&2;;
  esac
done

git add .
git commit -m $var
git push -u origin master
git push -u hub master