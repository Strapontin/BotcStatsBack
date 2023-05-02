import { Text } from "@nextui-org/react";

export default function RoleColored(props: { name: string; category: string }) {
  return (
    <Text b className={props.category}>
      {props.name}
    </Text>
  );
}
