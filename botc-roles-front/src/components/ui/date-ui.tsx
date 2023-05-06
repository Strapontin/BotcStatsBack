import { Text } from "@nextui-org/react";
import { DateToString } from "../helper/date";

export default function DateUi(props: { date: Date }) {
  const date = DateToString(props.date);

  return (
    <Text span color="yellow">
      {date}
    </Text>
  );
}
