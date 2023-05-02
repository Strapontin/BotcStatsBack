import { Image } from "@nextui-org/react";
import RoleColored from "./role-colored";

export default function ImageIconName(props: {
  name: string;
  category: string;
  setNameAtRightOfImage?: boolean;
}) {
  const imgPath = `/images/roles-icons/${props.name}.png`;

  var roleName = props.name.replace("-d-", " d'");
  roleName = roleName.replace("-l-", " l'");
  roleName = roleName.replace("-", " ");
  roleName = roleName.charAt(0).toUpperCase() + roleName.slice(1);

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
