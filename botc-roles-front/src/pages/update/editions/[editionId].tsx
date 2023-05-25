import { Fragment, useEffect, useState } from "react";
import Title from "@/components/ui/title";
import { updateEdition, getEditionById } from "../../../../data/back-api";
import { Loading, Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import EditionCreateEdit from "@/components/create-edit/edition-create-edit/EditionCreateEdit";
import { Edition, getNewEmptyEdition } from "@/entities/Edition";
import { useRouter } from "next/router";
import { toLowerRemoveDiacritics } from "@/helper/string";

export default function UpdateEditionPage() {
  const editionId: number = Number(useRouter().query.editionId);

  const [editionCreateEditKey, setEditionCreateEditKey] = useState(0);
  const [message, setMessage] = useState(<Fragment />);
  const [edition, setEdition] = useState<Edition>(getNewEmptyEdition());

  const [editions, setEditions] = useState<string[]>([]);

  useEffect(() => {
    if (editionId === undefined || isNaN(editionId)) return;

    async function initEdition() {
      const g = await getEditionById(editionId);
      setEdition(g);
    }
    initEdition();
  }, [editionId]);
  
  // Updates message on component refreshes
  useEffect(() => {
    if (edition.name === "" && edition.roles.length === 0) return;
    if (toLowerRemoveDiacritics(edition.name) === "") {
      updateMessage(true, "Un nom est obligatoire.");
    } else if (
      editions.filter(
        (p) =>
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(edition.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un module avec ce nom existe déjà.");
    } else {
      setMessage(<Fragment />);
    }
  }, [edition, editions]);

  if (edition.id === -1) {
    return (
      <Fragment>
        <Loading />
      </Fragment>
    );
  }

  const title = <Title>Modification du module {`'${edition.name}'`}</Title>;

  async function btnUpdateEdition() {
    if (!canUpdateEdition()) return;

    if (await updateEdition(edition.id, edition.name, edition.roles)) {
      const g = await getEditionById(editionId);
      setEdition(g);
      setEditionCreateEditKey(editionCreateEditKey + 1);
      updateMessage(false, `Le module a été modifié correctement.`);
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de la modification du module."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  function canUpdateEdition() {
    if (edition.name === "") {
      updateMessage(true, "Un nom est obligatoire.");
      return false;
    }

    if (
      editions.filter(
        (p) =>
          toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(edition.name)
      ).length !== 0
    ) {
      updateMessage(true, "Un module avec ce nom existe déjà.");
      return false;
    }

    return true;
  }

  return (
    <EditionCreateEdit
      key={editionCreateEditKey}
      title={title}
      edition={edition}
      setEdition={setEdition}
      message={message}
      btnPressed={btnUpdateEdition}
      btnText="Modifier le module"
    />
  );
}
