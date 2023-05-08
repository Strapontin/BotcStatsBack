import classes from "./ListItem.module.css";
import { Text } from "@nextui-org/react";

export default function ListItemTwoValues(props: {
  name: string;
  value1: string | number;
  value2: string | number;
  classValue1?: string;
  classValue2?: string;
}) {
  return (
    <div className={classes["list-item"]}>
      <Text span>{props.name}</Text>
      <div>
        <Text b className={props.classValue1}>
          {props.value1}
        </Text>{" "}
        |{" "}
        <Text b className={props.classValue2}>
          {props.value2}
        </Text>
      </div>
    </div>
  );
}
