import { Fragment } from "react";
import SelectionStats from "../select-stats/SelectionStats";
import classes from "./Layout.module.css";

export default function Layout(props: { children: any }) {
  return (
    <Fragment>
      <main className={classes.main}>
        <SelectionStats />
        {props.children}
      </main>
    </Fragment>
  );
}
