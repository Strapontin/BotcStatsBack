import classes from "./ListItem.module.css";

export default function ListItem(props: { children: any }) {
  return <div className={classes["list-item"]}>{props.children}</div>;
}
