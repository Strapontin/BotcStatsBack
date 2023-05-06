import { Image } from "@nextui-org/react";
import RoleColored from "./role-colored";

export default function ImageIconName(props: {
  name: string;
  category: string;
  setNameAtRightOfImage?: boolean;
}) {
  const imgFileName = props.name
    .normalize("NFD")
    .replace(/\p{Diacritic}/gu, "")
    .replaceAll(" ", "-");
  const imgPath = `/images/roles-icons/${imgFileName}.png`;
  console.log(imgFileName)

  var roleName = props.name;

  if (props.setNameAtRightOfImage) {
    return (
      <div className="flex ai-center">
        <Image width={50} height={50} src={imgPath} alt={props.name} />
        <RoleColored name={roleName} category={props.category} />
      </div>
    );
  } else {
    return (
      <div className="flex ai-center">
        <RoleColored name={roleName} category={props.category} />
        <Image width={50} height={50} src={imgPath} alt={props.name} />
      </div>
    );
  }
}
