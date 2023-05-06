import { Fragment, useEffect, useState } from "react";
import { getPlayerByName } from "../../../data/dummy-backend";
import { Player } from "@/entities/Player";
import { useRouter } from "next/router";
import Title from "@/components/ui/title";
import { Collapse, Loading, Spacer } from "@nextui-org/react";

export default function PlayerPage() {
  const title = <Title>Création d{"'"}une nouvelle partie</Title>;

  return (
    <Fragment>
      {title}
      <Spacer y={3} />
      <Loading />
    </Fragment>
  );

  return <Fragment>{title}</Fragment>;
}
