import classes from "./ListItem.module.css";
import { Text } from "@nextui-org/react";

export default function ListItem(props: {
  name: string;
  value?: any;
  subName?: string;
}) {
  return (
    <div className={classes["list-item"]}>
      <div className={classes["left-side"]}>
        <Text span>{props.name}</Text>
        <Text span className={classes["subname"]} size={13}>
          {props.subName}
        </Text>
      </div>
      <Text span>{props.value}</Text>
    </div>
  );
}
