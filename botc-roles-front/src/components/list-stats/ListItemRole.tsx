import { CharacterType } from "@/entities/enums/characterType";
import ImageIconName from "../ui/image-icon-name";
import classes from "./ListItem.module.css";
import { Text } from "@nextui-org/react";

export default function ListItemRole(props: {
  image: string;
  characterType: CharacterType;
  nbWins: number;
  nbLoses: number;
  nbGamesPlayed: number;
}) {
  return (
    <div className={classes["list-item"]}>
      <div>
        <ImageIconName
          setNameAtLeftOfImage
          name={props.image}
          characterType={props.characterType}
        />
      </div>
      <div>
        <Text b className="green">
          {props.nbWins}
        </Text>{" "}
        |{" "}
        <Text b className="red">
          {props.nbLoses}
        </Text>{" "}
        |{" "}
        <Text b span>
          {props.nbGamesPlayed}
        </Text>
      </div>
    </div>
  );
}
