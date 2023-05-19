import classes from "./ListItem.module.css";
import { Text } from "@nextui-org/react";

export default function ListItem(props: {
  name: string;
  value?: any;
  subName?: string;
  onPress?: any;
}) {
  var timeStamp: number;

  function onTouchStart(e: any) {
    timeStamp = e.timeStamp;
  }

  function onTouchMove(e: any) {
    timeStamp = NaN;
  }

  function onTouchEnd(e: any) {
    if (e.timeStamp - timeStamp < 500) {
      props.onPress();
    }
  }

  return (
    <div
      className={classes["list-item"]}
      onClick={props.onPress}
      onTouchStart={onTouchStart}
      onTouchEnd={onTouchEnd}
      onTouchMove={onTouchMove}
    >
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
