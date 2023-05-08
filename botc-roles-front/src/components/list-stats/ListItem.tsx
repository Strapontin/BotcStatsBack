import classes from "./ListItem.module.css";
import { Text } from "@nextui-org/react";

export default function ListItem(props: { name: any; value: any }) {

  return (
    <div className={classes["list-item"]}>
      <Text span>{props.name}</Text>
      <Text span>{props.value}</Text>
    </div>
  );
}
