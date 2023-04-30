import classes from "./ListItem.module.css";

export default function ListItem(props: { name: string; value: number }) {
  return (
    <div className={classes["list-item"]}>
      <span className="text-lg">{props.name}</span>
      <span className="text-lg">{props.value}</span>
    </div>
  );
}
