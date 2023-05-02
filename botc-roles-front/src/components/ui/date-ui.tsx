import { Text } from "@nextui-org/react";

export default function DateUi(props: { date: Date }) {
  const d = new Date(props.date);

  const day = d.getDate().toString().padStart(2, "0");
  const month = (d.getMonth() + 1).toString().padStart(2, "0");
  const year = d.getFullYear();

  const date = `${day}/${month}/${year}`;

  return (
    <Text span color="yellow">
      {date}
    </Text>
  );
}
